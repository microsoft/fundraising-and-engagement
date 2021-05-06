using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace FundraisingandEngagement.DataverseSynchronization
{
    /// <summary>
    /// Represents a (persistent) repository for storing a representation of Dataverse rows (formerly CDS records).
    /// On the API level, rows are represented as Microsoft.Xrm.Sdk.Entity objects for interoperability with Microsoft.Xrm.Sdk.IOrganizationService.
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// Update an entity or create it if it doesn't exist yet.
        /// The combination of Entity.LogicalName and Entity.Id are expected to act as the primary key.
        /// </summary>
        void Upsert(IEnumerable<Entity> changedEntities);

        /// <summary>
        /// Delete an entity with the given logical name and ID
        /// </summary>
        void Delete(string entityLogicalName, Guid entityId);

        /// <summary>
        /// Delete all entities of given type except those with given IDs to retain.
        /// </summary>
        void RetainOnly(string entityLogicalName, IEnumerable<Guid> entityIdsToRetain);
    }
}