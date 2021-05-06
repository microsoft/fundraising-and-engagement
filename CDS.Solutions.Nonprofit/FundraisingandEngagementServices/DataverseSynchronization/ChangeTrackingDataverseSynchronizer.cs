using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace FundraisingandEngagement.DataverseSynchronization
{
    /// <summary>
    /// Synchronizes data from Dataverse to an underlying repository by polling the Dataverse change tracking API
    /// (see https://docs.microsoft.com/en-us/powerapps/developer/data-platform/use-change-tracking-synchronize-data-external-systems ).
    /// All tables to be synchronized must have change tracking enabled
    /// (see https://docs.microsoft.com/en-us/previous-versions/dynamicscrm-2016/administering-dynamics-365/dn946903(v=crm.8) ).
    ///
    /// The implementation is not thread-safe.
    /// </summary>
    public class ChangeTrackingDataverseSynchronizer : IDataverseSynchronizer
    {
        private readonly IOrganizationService _organizationService;
        private readonly IEntityRepository _entityRepository;
        private readonly IDataVersionTokenRepository _dataVersionTokenRepository;
        private readonly ILogger _logger;
        private readonly int _pollPageSize;

        public delegate void FullSyncDelegate(string entityToSync);

        public FullSyncDelegate OnBeforeFullSync;


        public ChangeTrackingDataverseSynchronizer(IOrganizationService organizationService, IEntityRepository entityRepository, IDataVersionTokenRepository dataVersionTokenRepository, ILogger logger, int pollPageSize = 500)
        {
            _organizationService = organizationService;
            _entityRepository = entityRepository;
            _dataVersionTokenRepository = dataVersionTokenRepository;
            _logger = logger;
            _pollPageSize = pollPageSize;
        }

        public void Synchronize(IEnumerable<string> entitiesToSynchronize)
        {
            var entities = entitiesToSynchronize.ToList();
            this._logger.LogInformation("Synchronizing Dataverse entities {entities}", String.Join(", ", entities));
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var partialSyncResultsInOrder = new List<PartialSyncResult>();

            // First apply all Updates/Creates
            foreach (var entity in entities)
            {
                var currentDataVersionToken = this._dataVersionTokenRepository.Get(entity);
                PartialSyncResult partialSyncResult;
                if (currentDataVersionToken == null)
                {
                    this._logger.LogInformation("No token for entity {entity}, starting a full synchronization", entity);
                    partialSyncResult = FullSync(entity);
                }
                else
                {
                    try
                    {
                        partialSyncResult = DiffSync(entity, currentDataVersionToken);
                    }
                    catch (TokenExpiredException)
                    {
                        this._logger.LogWarning("Token {token} for entity {entity} has expired, starting a full synchronization", currentDataVersionToken, entity);
                        partialSyncResult = FullSync(entity);
                    }
                }

                partialSyncResultsInOrder.Add(partialSyncResult);
            }

            // Then apply all Deletes in reverse order;
            // then it's safe to persist the new token
            foreach (var partialSyncResult in Enumerable.Reverse(partialSyncResultsInOrder))
            {
                partialSyncResult.ExecuteDelayedDeleteAction();
                this._logger.LogDebug("Executed deletes for {entity}", partialSyncResult.EntityName);

                this._dataVersionTokenRepository.Put(partialSyncResult.EntityName, partialSyncResult.NextDataVersionToken);
                this._logger.LogInformation("Synchronized {entity}, updated DataToken to {token}", partialSyncResult.EntityName, partialSyncResult.NextDataVersionToken);
            }
            stopWatch.Stop();
            this._logger.LogInformation($"Synchronized Dataverse entities in {stopWatch.Elapsed}");
        }

        /// <summary>
        /// Perform synchronization based on diff from Change Tracking API
        /// </summary>
        private PartialSyncResult DiffSync(string entityToSynchronize, string currentDataVersionToken)
        {
            int updatesCount = 0;
            RetrieveEntityChangesResponse response = null;
            var idsToDelete = new List<Guid>();
            for (var pageNumber = 1; response == null || response.EntityChanges.MoreRecords; pageNumber++)
            {
                var pagingCookie = response?.EntityChanges.PagingCookie;
                response = makeRequest(entityToSynchronize, currentDataVersionToken, pageNumber, pagingCookie);
                var changesByType = response.EntityChanges.Changes
                    .GroupBy(it => it.Type);

                foreach (var changeGroup in changesByType)
                {
                    switch (changeGroup.Key)
                    {
                        case ChangeType.NewOrUpdated:
                            this._entityRepository.Upsert(
                                from change in changeGroup
                                select ValidateNewOrUpdatedEntity(change)
                            );
                            break;
                        case ChangeType.RemoveOrDeleted:
                            idsToDelete.AddRange(
                                from change in changeGroup
                                select ValidateRemovedEntity(change, entityToSynchronize).Id
                            );
                            break;
                        default:
                            throw new DataverseSynchronizationException("Unexpected change type: " + changeGroup.Key);
                    }
                }

                updatesCount += response.EntityChanges.Changes.Count;
                LogProgress(entityToSynchronize, updatesCount);
            }

            _logger.LogDebug("Synchronized changes from Dataverse, {updatesCount} changes received", updatesCount);
            return new PartialSyncResult(
                entityToSynchronize,
                response.EntityChanges.DataToken,
                () => this.DeleteAll(entityToSynchronize, idsToDelete)
            );
        }

        /// <summary>
        /// Perform full synchronization - download all records from Dataverse, insert them into repository, and delete everything that was NOT received.
        /// </summary>
        private PartialSyncResult FullSync(string entityToSynchronize)
        {
            if (this.OnBeforeFullSync != null)
            {
                this.OnBeforeFullSync(entityToSynchronize);
            }

            RetrieveEntityChangesResponse response = null;
            var allResponseIds = new HashSet<Guid>();
            for (var pageNumber = 1; response == null || response.EntityChanges.MoreRecords; pageNumber++)
            {
                var pagingCookie = response?.EntityChanges.PagingCookie;
                response = makeRequest(entityToSynchronize, null, pageNumber, pagingCookie);

                var changesByType = response.EntityChanges.Changes
                    .GroupBy(it => it.Type);

                foreach (var changeGroup in changesByType)
                {
                    switch (changeGroup.Key)
                    {
                        case ChangeType.NewOrUpdated:
                            this._entityRepository.Upsert(
                                from change in changeGroup select ValidateNewOrUpdatedEntity(change)
                            );

                            AddRangeTo(
                                allResponseIds,
                                from change in changeGroup select ValidateNewOrUpdatedEntity(change).Id
                            );
                            break;
                        default:
                            throw new DataverseSynchronizationException("Unexpected change type for full synchronization: " + changeGroup.Key);
                    }
                }

                LogProgress(entityToSynchronize, allResponseIds.Count);
            }

            _logger.LogDebug("Synchronized entities from Dataverse, {updatesCount} entities received", allResponseIds.Count);
            return new PartialSyncResult(
                entityToSynchronize,
                response.EntityChanges.DataToken,
                () => this._entityRepository.RetainOnly(entityToSynchronize, allResponseIds)
            );
        }

        private void LogProgress(string entity, int count)
        {
            _logger.LogInformation("... processed {count} updates for entity {entity}", count, entity);
        }

        private RetrieveEntityChangesResponse makeRequest(string entityToSynchronize, string dataVersionToken, int pageNumber = 1, string pagingCookie = null)
        {
            var request = new RetrieveEntityChangesRequest
            {
                EntityName = entityToSynchronize,
                Columns = new ColumnSet(true),
                DataVersion = dataVersionToken,
                PageInfo = new PagingInfo { Count = _pollPageSize, PageNumber = pageNumber, ReturnTotalRecordCount = false, PagingCookie = pagingCookie }
            };

            try
            {
                var response = this._organizationService.Execute(request) as RetrieveEntityChangesResponse;
                if (response == null)
                {
                    throw new DataverseSynchronizationException($"Unexpected response type for {nameof(RetrieveEntityChangesRequest)}: {response}");
                }

                return response;
            }
            catch (FaultException e) when (e.Message.Contains("expired") && e.Message.Contains("perform a full sync"))
            {
                throw new TokenExpiredException(e);
            }
            catch (Exception e)
            {
                throw new DataverseSynchronizationException("Error while synchronizing entities of type " + entityToSynchronize, e);
            }
        }

        private void DeleteAll(string entityName, IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                this._entityRepository.Delete(entityName, id);
            }
        }

        private static Entity ValidateNewOrUpdatedEntity(IChangedItem changedItem)
        {
            if (changedItem.Type != ChangeType.NewOrUpdated)
            {
                throw new ArgumentException($"Expecting ${ChangeType.NewOrUpdated} item type, {changedItem.Type} given");
            }

            if (!(changedItem is NewOrUpdatedItem typedItem))
            {
                throw new DataverseSynchronizationException($"Received unexpected change for type {changedItem.Type}: {changedItem.GetType()}");
            }

            return typedItem.NewOrUpdatedEntity;
        }

        private EntityReference ValidateRemovedEntity(IChangedItem changedItem, string expectedEntityName)
        {
            if (changedItem.Type != ChangeType.RemoveOrDeleted)
            {
                throw new ArgumentException($"Expecting ${ChangeType.RemoveOrDeleted} item type, {changedItem.Type} given");
            }

            if (!(changedItem is RemovedOrDeletedItem typedItem))
            {
                throw new DataverseSynchronizationException($"Received unexpected change for type {changedItem.Type}: {changedItem.GetType()}");
            }

            if (typedItem.RemovedItem.LogicalName != expectedEntityName)
            {
                throw new DataverseSynchronizationException(
                    $"Unexpected response: got delete of {typedItem.RemovedItem.LogicalName} with id {typedItem.RemovedItem.Id} when asked for changes of entity {expectedEntityName}");
            }

            return typedItem.RemovedItem;
        }

        private static void AddRangeTo<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        private class TokenExpiredException : DataverseSynchronizationException
        {
            public TokenExpiredException(FaultException innerException) : base("Data version token expired", innerException) { }
        }

        private struct PartialSyncResult
        {
            private Action _delayedDeleteAction;
            public string NextDataVersionToken { get; }
            public string EntityName { get; }

            public PartialSyncResult(string entityName, string nextDataVersionToken, Action delayedDeleteAction)
            {
                EntityName = entityName;
                NextDataVersionToken = nextDataVersionToken;
                _delayedDeleteAction = delayedDeleteAction;
            }

            public void ExecuteDelayedDeleteAction()
            {
                _delayedDeleteAction.Invoke();
            }
        }
    }
}