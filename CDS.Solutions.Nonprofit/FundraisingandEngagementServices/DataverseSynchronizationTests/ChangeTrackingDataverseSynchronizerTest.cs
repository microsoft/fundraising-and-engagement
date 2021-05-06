using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using DataverseSynchronizationTests;
using FluentAssertions;
using FundraisingandEngagement.DataverseSynchronization;
using MELT;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using NUnit.Framework;

namespace FundraisingandEngagement.DataverseSynchronizationTests
{
    public class ChangeTrackingDataverseSynchronizerTest
    {
        private const string DesignationEntity = "msnfp_designation";
        private static readonly NullLogger<ChangeTrackingDataverseSynchronizer> NullLogger = new NullLogger<ChangeTrackingDataverseSynchronizer>();

        private InMemoryEntityRepository entityRepository;
        private Mock<IOrganizationService> mockCrmClient;
        private InMemoryDataVersionTokenRepository tokenRepository;

        [SetUp]
        public void Setup()
        {
            entityRepository = new InMemoryEntityRepository();
            tokenRepository = new InMemoryDataVersionTokenRepository();
            mockCrmClient = new Mock<IOrganizationService>(MockBehavior.Strict);
        }

        [Test]
        public void SynchronizesEntityOnFirstSync()
        {
            // Arrange
            var entity1 = createDesignationEntity("designation1");
            var entity2 = createDesignationEntity("designation2");

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("token1", newOrUpdatedItem(entity1), newOrUpdatedItem(entity2)));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(2).And
                .Contain(new[] { entity1, entity2 });
        }

        [Test]
        public void SynchronizesDiffAfterPreviousSync()
        {
            // Arrange
            var initialEntity = createDesignationEntity("initial");
            var removedEntity = createDesignationEntity("removed");
            var updatedEntity = createDesignationEntity("updated");
            var createdEntity = createDesignationEntity("created");
            var diffToken = "token1";

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse(diffToken, newOrUpdatedItem(initialEntity), newOrUpdatedItem(removedEntity)));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, diffToken)))
                .Returns(createChangesResponse("anotherToken", newOrUpdatedItem(createdEntity), newOrUpdatedItem(updatedEntity), removedOrDeletedItem(removedEntity)));
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(3).And
                .Contain(new[] { initialEntity, updatedEntity, createdEntity });
        }

        [Test]
        public void SynchronizesDiffForMultipleEntities()
        {
            // Arrange
            var designation1 = createDesignationEntity("designation1");
            var designation2 = createDesignationEntity("designation2");
            var designationToken = "tokenD";

            var contact1 = new Entity("contact", Guid.NewGuid()) { Attributes = new AttributeCollection { ["name"] = "contact1" } };
            var contact2 = new Entity("contact", Guid.NewGuid()) { Attributes = new AttributeCollection { ["name"] = "contact2" } };
            var contactToken = "tokenC";

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse(designationToken, newOrUpdatedItem(designation1)));
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest("contact", null)))
                .Returns(createChangesResponse(contactToken, newOrUpdatedItem(contact1)));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity, "contact" });

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, designationToken)))
                .Returns(createChangesResponse(designationToken, newOrUpdatedItem(designation2)));
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest("contact", contactToken)))
                .Returns(createChangesResponse(contactToken, newOrUpdatedItem(contact2)));

            synchronizer.Synchronize(new HashSet<string> { DesignationEntity, "contact" });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(4).And
                .Contain(new[] { designation1, designation2, contact1, contact2 });
        }

        [Test]
        public void SynchronizesOverMultiplePages()
        {
            // Arrange
            var entity1 = createDesignationEntity("designation1");
            var entity2 = createDesignationEntity("designation2");
            var entity3 = createDesignationEntity("designation3");
            var entity4 = createDesignationEntity("designation4");
            var pageSize = 2;
            var pageCookie1 = "cookie1";
            var dataVersionToken1 = "token1";

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null, new PagingInfo { Count = pageSize, PageNumber = 1 })))
                .Returns(createChangesResponse(null, pageCookie1, true, newOrUpdatedItem(entity1), newOrUpdatedItem(entity2)));

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null, new PagingInfo { Count = pageSize, PageNumber = 2, PagingCookie = pageCookie1 })))
                .Returns(createChangesResponse(dataVersionToken1, pageCookie1, false, newOrUpdatedItem(entity3)));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger, pageSize);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, dataVersionToken1, new PagingInfo { Count = pageSize, PageNumber = 1 })))
                .Returns(createChangesResponse("token2", newOrUpdatedItem(entity4)));

            synchronizer.Synchronize(new HashSet<string> { DesignationEntity }); // testing second synchronization to make sure dataToken was captured correctly

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(4).And
                .Contain(new[] { entity1, entity2, entity3, entity4 });
        }

        [Test]
        public void SynchronizesAfterTokenExpiry()
        {
            // Arrange
            var initialEntity = createDesignationEntity("initial");
            var removedEntity = createDesignationEntity("removed");
            var createdEntity = createDesignationEntity("created");
            var createdEntity2 = createDesignationEntity("created2");
            var expiredToken = "expiredToken1";
            var newToken = "newToken1";

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse(expiredToken, newOrUpdatedItem(initialEntity), newOrUpdatedItem(removedEntity)));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            // Initial sync, this will initialize versionToken in synchronizer
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Simulate expiry of versionToken
            this.mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, expiredToken)))
                .Throws(new FaultException<OrganizationServiceFault>(
                    new OrganizationServiceFault { Message = "Version stamp associated with the client has expired.Please perform a full sync." },
                    new FaultReason("Version stamp associated with the client has expired. Please perform a full sync.")
                ));

            // Simulate that an item was created and removed since last sync
            this.mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse(newToken, newOrUpdatedItem(initialEntity), newOrUpdatedItem(createdEntity)));

            // Run another sync that deals with the expiration
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Run yet another sync to verify that new diff token was captured correctly
            this.mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, newToken)))
                .Returns(createChangesResponse("newNewToken", newOrUpdatedItem(createdEntity2)));
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(3).And
                .Contain(new[] { initialEntity, createdEntity, createdEntity2 });
        }

        [Test]
        public void SynchronizesWhenTokenMissingButRepositoryContainsData()
        {
            // Arrange
            var entityOnlyInAzure = createDesignationEntity("azure");
            var entityOnlyInDataverse = createDesignationEntity("dataverse");
            var entityInBoth = createDesignationEntity("both");

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("token1", newOrUpdatedItem(entityOnlyInDataverse), newOrUpdatedItem(entityInBoth)));
            this.entityRepository.Upsert(new[] { entityOnlyInAzure });
            this.entityRepository.Upsert(new[] { entityInBoth });

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(2).And
                .Contain(new[] { entityOnlyInDataverse, entityInBoth });
        }

        [Test]
        public void SynchronizesDeletedItemNotPresentInRepository()
        {
            // Arrange
            var removedEntity = createDesignationEntity("removed");
            var createdEntity = createDesignationEntity("created");
            var diffToken = "token1";

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse(diffToken));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, diffToken)))
                .Returns(createChangesResponse("anotherToken", newOrUpdatedItem(createdEntity), removedOrDeletedItem(removedEntity)));

            // Act
            // Verify that RemovedOrDelete item in diff doesn't cause exception or other unexpected behavior even when it's not present in the repository
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            entityRepository.AllEntities.Should()
                .HaveCount(1).And
                .Contain(new[] { createdEntity });
        }

        [Test]
        public void SynchronizesUpdatesInGivenOrderAndDeletesInReverseForDiff()
        {
            // Arrange
            var removedDesignation = createDesignationEntity("removed designation");
            var updatedDesignation = createDesignationEntity("updated designation");
            var removedContact = new Entity("contact", Guid.NewGuid()) { Attributes = new AttributeCollection { ["name"] = "removed contact" } };
            var updatedContact = new Entity("contact", Guid.NewGuid()) { Attributes = new AttributeCollection { ["name"] = "updated contact" } };

            tokenRepository.Put("msnfp_designation", "designationToken1");
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, "designationToken1")))
                .Returns(createChangesResponse("anotherToken", newOrUpdatedItem(updatedDesignation), removedOrDeletedItem(removedDesignation)));
            tokenRepository.Put("contact", "contactToken1");
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest("contact", "contactToken1")))
                .Returns(createChangesResponse("contactToken1", removedOrDeletedItem(removedContact), newOrUpdatedItem(updatedContact)));

            var sequence = new MockSequence();
            var entityRepositoryMock = new Mock<IEntityRepository>(MockBehavior.Strict);
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Upsert(new[] { updatedDesignation }));
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Upsert(new[] { updatedContact }));
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Delete("contact", removedContact.Id));
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Delete("msnfp_designation", removedDesignation.Id));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepositoryMock.Object, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new List<string> { "msnfp_designation", "contact" }); // Order in the list matters here

            // Assert
            entityRepositoryMock.VerifyAll();
        }

        [Test]
        public void SynchronizesUpdatesInGivenOrderAndDeletesInReverseForFullSync()
        {
            // Arrange
            var updatedDesignation = createDesignationEntity("updated designation");
            var updatedContact = new Entity("contact", Guid.NewGuid()) { Attributes = new AttributeCollection { ["name"] = "updated contact" } };

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("x", newOrUpdatedItem(updatedDesignation)));
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest("contact", null)))
                .Returns(createChangesResponse("y", newOrUpdatedItem(updatedContact)));

            var sequence = new MockSequence();
            var entityRepositoryMock = new Mock<IEntityRepository>(MockBehavior.Strict);
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Upsert(new[] { updatedDesignation }));
            entityRepositoryMock.InSequence(sequence).Setup(x => x.Upsert(new[] { updatedContact }));

            entityRepositoryMock.InSequence(sequence).Setup(x => x.RetainOnly("contact", new List<Guid> { updatedContact.Id }));
            entityRepositoryMock.InSequence(sequence).Setup(x => x.RetainOnly("msnfp_designation", new List<Guid> { updatedDesignation.Id }));

            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepositoryMock.Object, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new List<string> { "msnfp_designation", "contact" }); // Order in the list matters here

            // Assert
            entityRepositoryMock.VerifyAll();
        }

        [Test]
        public void LogsSynchronizationResults()
        {
            // Arrange
            var entity1 = createDesignationEntity("designation1");
            var loggerFactory = TestLoggerFactory.Create();

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("token1", newOrUpdatedItem(entity1)));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, loggerFactory.CreateLogger<ChangeTrackingDataverseSynchronizer>());

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            loggerFactory.Sink.LogEntries.Should()
                .Contain(e => "Synchronized msnfp_designation, updated DataToken to token1".Equals(e.Message));
        }

        [Test]
        public void PersistsTokensBetweenSynchronizations()
        {
            // Arrange
            var tokenBefore = "token1";
            var tokenAfter = "token2";
            tokenRepository.Put(DesignationEntity, tokenBefore);

            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, tokenBefore)))
                .Returns(createChangesResponse(tokenAfter));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity }); // Get token

            // Assert
            tokenRepository.Get(DesignationEntity).Should().Be(tokenAfter);
        }

        [Test]
        public void ThrowsOnRequestError()
        {
            // Arrange
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Throws(new IOException("test exception"));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);

            // Act
            synchronizer.Invoking(s => s.Synchronize(new HashSet<string> { DesignationEntity }))
                .Should().Throw<Exception>()
                .WithMessage("Error while synchronizing entities of type " + DesignationEntity);
        }

        [Test]
        public void DoesNotDoFullSyncIfBeforeFullSyncHandlerThrows()
        {
            // Arrange
            var entity1 = createDesignationEntity("designation1");
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("token1", newOrUpdatedItem(entity1)));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);
            synchronizer.OnBeforeFullSync += (e) => { throw new Exception("Push failed"); };

            // Act
            Assert.Throws<Exception>(() => synchronizer.Synchronize(new HashSet<string> { DesignationEntity }));

            // Assert that the sync didn't happen
            entityRepository.AllEntities.Should()
                .HaveCount(0);
        }

        public void CallsOnBeforeFullSyncBeforeFullSync()
        {
            // Arrange
            var entity1 = createDesignationEntity("designation1");
            mockCrmClient.Setup(client => client.Execute(IsOrganizationRequest(DesignationEntity, null)))
                .Returns(createChangesResponse("token1", newOrUpdatedItem(entity1)));
            var synchronizer = new ChangeTrackingDataverseSynchronizer(mockCrmClient.Object, entityRepository, tokenRepository, NullLogger);
            bool onBeforeFullSyncWasCalled = false;
            synchronizer.OnBeforeFullSync += (e) => { onBeforeFullSyncWasCalled = true; };

            // Act
            synchronizer.Synchronize(new HashSet<string> { DesignationEntity });

            // Assert
            Assert.IsTrue(onBeforeFullSyncWasCalled);
        }


        // Rate limits should be handled automatically: https://docs.microsoft.com/en-us/powerapps/developer/data-platform/api-limits#using-the-organization-service

        private static OrganizationRequest IsOrganizationRequest(string entityName, string dataVersionToken)
        {
            return It.Is<OrganizationRequest>(request =>
                (request as RetrieveEntityChangesRequest).EntityName == entityName
                && (request as RetrieveEntityChangesRequest).Columns.AllColumns
                && (request as RetrieveEntityChangesRequest).DataVersion == dataVersionToken
            );
        }

        private static OrganizationRequest IsOrganizationRequest(string entityName, string dataVersionToken, PagingInfo pagingInfo)
        {
            return It.Is<OrganizationRequest>(request =>
                (request as RetrieveEntityChangesRequest).EntityName == entityName
                && (request as RetrieveEntityChangesRequest).Columns.AllColumns
                && (request as RetrieveEntityChangesRequest).DataVersion == dataVersionToken
                && PagingCookieEquals((request as RetrieveEntityChangesRequest).PageInfo, pagingInfo)
            );
        }

        private static bool PagingCookieEquals(PagingInfo info1, PagingInfo info2)
        {
            if (info1 == info2)
            {
                return true;
            }

            if (info1 == null || info2 == null)
            {
                return false;
            }

            return info1.PageNumber == info2.PageNumber && info1.Count == info2.Count && info1.PagingCookie == info2.PagingCookie && info1.ReturnTotalRecordCount == info2.ReturnTotalRecordCount;
        }

        private static NewOrUpdatedItem newOrUpdatedItem(Entity entity)
        {
            return new NewOrUpdatedItem(ChangeType.NewOrUpdated, entity);
        }

        private static RemovedOrDeletedItem removedOrDeletedItem(Entity entity)
        {
            return new RemovedOrDeletedItem(ChangeType.RemoveOrDeleted, new EntityReference(entity.LogicalName, entity.Id));
        }

        private static RetrieveEntityChangesResponse createChangesResponse(string dataToken, params IChangedItem[] changes)
        {
            var entityChanges = new BusinessEntityChanges { Changes = new BusinessEntityChangesCollection(changes), DataToken = dataToken, MoreRecords = false };
            return new RetrieveEntityChangesResponse { Results = new ParameterCollection { [nameof(RetrieveEntityChangesResponse.EntityChanges)] = entityChanges } };
        }

        private static RetrieveEntityChangesResponse createChangesResponse(string dataToken, string pagingCookie, bool moreRecords, params IChangedItem[] changes)
        {
            var entityChanges = new BusinessEntityChanges { Changes = new BusinessEntityChangesCollection(changes), DataToken = dataToken, MoreRecords = moreRecords, PagingCookie = pagingCookie };
            return new RetrieveEntityChangesResponse { Results = new ParameterCollection { [nameof(RetrieveEntityChangesResponse.EntityChanges)] = entityChanges } };
        }

        private static Entity createDesignationEntity(string nameAttribute)
        {
            return new Entity(DesignationEntity, Guid.NewGuid()) { Attributes = new AttributeCollection { ["msnfp_name"] = nameAttribute } };
        }
    }
}