using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Services;
using FundraisingandEngagement.Services.DataPush;
using FundraisingandEngagement.Services.Xrm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Xrm.Crm.WebApi.Models;

namespace BackgroundServicesTests
{
    public class FullSyncHandlerTests
    {
        private static readonly NullLogger<DbEntityRepository> NullLogger = new NullLogger<DbEntityRepository>();
        private DbContextOptions<PaymentContext> dbContextOptions;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<PaymentContext>()
                .UseInMemoryDatabase("test")
                .ConfigureWarnings(it => it.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            using var context = new PaymentContext(this.dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public void FullSyncHandlerPushesDirtyRecords()
        {
            using (var dbContext = new PaymentContext(dbContextOptions))
            {
                // Add a 'dirty' transaction which will be pushed
                var id = Guid.NewGuid();
                dbContext.Transaction.Add(new Transaction() { TransactionId = id, SyncDate = null });
                dbContext.SaveChanges();
                var xrmService = new MockXrmService();
                var dataPushService = new DataPushService(dbContext, xrmService, NullLogger);
                var entitiesSequenceBackgroundService = new EntitiesSequenceBackgroundService(dataPushService, NullLogger);
                var sut = new FullSyncHandler(entitiesSequenceBackgroundService, NullLogger);

                sut.PushEntitiesToDataverseSynchronously("msnfp_transaction");

                Assert.IsNotNull(xrmService.Pushed.Find(p => p.LogicalName == "msnfp_transaction" && p.Id == id), "Expected msnfp_transaction to be pushed");
            }
        }

        [Test]
        public void FullSyncHandlerPushesDirtyRecordsOnlyOnce()
        {
            using (var dbContext = new PaymentContext(dbContextOptions))
            {
                // Add a 'dirty' transaction which will be pushed
                var id = Guid.NewGuid();
                dbContext.Transaction.Add(new Transaction() { TransactionId = id, SyncDate = null });
                dbContext.SaveChanges();
                var xrmService = new MockXrmService();
                var dataPushService = new DataPushService(dbContext, xrmService, NullLogger);
                var entitiesSequenceBackgroundService = new EntitiesSequenceBackgroundService(dataPushService, NullLogger);
                var sut = new FullSyncHandler(entitiesSequenceBackgroundService, NullLogger);

                // Make the first push
                sut.PushEntitiesToDataverseSynchronously("msnfp_transaction");
                Assert.AreEqual(1, xrmService.Pushed.Count(p => p.LogicalName == "msnfp_transaction" && p.Id == id), "Expected msnfp_transaction to be pushed once");

                // Mark the transaction dirty again
                var transaction = dbContext.Transaction.Where(t => t.TransactionId == id).First();
                transaction.SyncDate = null;
                dbContext.Transaction.Update(transaction);
                dbContext.SaveChanges();

                // Try pushing again - by design, the FullSyncHandler should push only once
                sut.PushEntitiesToDataverseSynchronously("msnfp_transaction");
                Assert.AreEqual(1, xrmService.Pushed.Count(p => p.LogicalName == "msnfp_transaction" && p.Id == id), "Expected msnfp_transaction not to be pushed again");
            }
        }

        // Some of the methods of IXrmService are extension methods and these
        // cannot be mocked with Mock<>, hence a full mock class implementation.
        private class MockXrmService : IXrmService
        {
            public List<Entity> Pushed = new List<Entity>();

            public Task<Guid> CreateAsync(Entity crmEntity)
            {
                this.Pushed.Add(crmEntity);
                return Task<Guid>.FromResult(crmEntity.Id);
            }

            public Task UpdateAsync(Entity crmEntity)
            {
                this.Pushed.Add(crmEntity);
                return Task<Guid>.FromResult(crmEntity.Id);
            }

            public Task<Entity> GetAsync(string logicalName, Guid id, params string[] properties)
            {
                return Task<Guid>.FromResult<Entity>(new Entity(logicalName) { Id = id });
            }
            public Task DisassociateAsync(Entity crmEntity, string navigationProperty)
            {
                throw new NotImplementedException();
            }

            public Task<IReadOnlyList<Entity>> GetFilteredListAsync(string entityCollection, string filter, params string[] properties)
            {
                throw new NotImplementedException();
            }

            public Task<IReadOnlyList<Entity>> GetListAsync(string entityCollection, params string[] properties)
            {
                throw new NotImplementedException();
            }
        }
    }
}