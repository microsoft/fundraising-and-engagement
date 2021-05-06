using System;
using System.Collections.Generic;
using Data.Tests;
using Data.Tests.Utils;
using FundraisingandEngagement;
using FundraisingandEngagement.BackgroundServices;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace BackgroundServicesTests
{
    public class YearlyGivingFromEntityTests
    {
        private static readonly NullLogger<YearlyGivingFromEntity> NullLogger = new NullLogger<YearlyGivingFromEntity>();
        private static readonly NullLoggerFactory NullLoggerFactory = new NullLoggerFactory();

        [Test]
        public void YearlyGivingFromEntitySmokeTest()
        {
            var contactId = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var contacts = new Contact[] { new Contact() { ContactId = contactId } };
            var dbContext = createDbContextMock()
                .WithTransactions(new Transaction[] {
                    new Transaction() { TransactionId = transactionId, Amount = 101, BookDate = DateTime.Today, StatusCode = StatusCode.Completed, TypeCode = TransactionTypeCode.Donation, CustomerId = contactId, CustomerIdType = CustomerIdType.Contact },
                    new Transaction() { TransactionId = Guid.NewGuid(), Amount = 102, BookDate = DateTime.Today, StatusCode = StatusCode.Completed, TypeCode = TransactionTypeCode.Donation, CustomerId = contactId, CustomerIdType = CustomerIdType.Contact },
                })
                .WithContacts(contacts)
                .WithDonorCommitments(new DonorCommitment[0])
                .WithEventPackages(new EventPackage[0]).Build();
            var synchronizer = new Mock<IDbEntitySynchronizer>();
            var calculator = new YearlyGivingCalculator(dbContext.Object, NullLogger, synchronizer.Object);
            var sut = new YearlyGivingFromEntity(dbContext.Object, NullLogger, calculator);

            sut.CalculateFromPaymentEntity(transactionId, "msnfp_transaction");

            Assert.AreEqual(203, contacts[0].msnfp_year0_giving);
            Assert.AreEqual(203, contacts[0].msnfp_lifetimegivingsum);
            synchronizer.Verify(s => s.SynchronizeEntitiesToDbTransitively(It.IsAny<List<Type>>()), Times.Once());
        }

        [Test]
        public void UpdatesGivenAccount()
        {
            var entityId = Guid.NewGuid();
            var dbContext = createDbContextMock().Build();
            var calculator = new Mock<IYearlyGivingCalculator>();
            var sut = new YearlyGivingFromEntity(dbContext.Object, NullLogger, calculator.Object);

            sut.CalculateFromPaymentEntity(entityId, "account");

            calculator.Verify(it => it.UpdateCustomer(entityId, CustomerIdType.Account));
        }

        [Test]
        public void UpdatesGivenContact()
        {
            var entityId = Guid.NewGuid();
            var dbContext = createDbContextMock().Build();
            var calculator = new Mock<IYearlyGivingCalculator>();
            var sut = new YearlyGivingFromEntity(dbContext.Object, NullLogger, calculator.Object);

            sut.CalculateFromPaymentEntity(entityId, "contact");

            calculator.Verify(it => it.UpdateCustomer(entityId, CustomerIdType.Contact));
        }

        [Test]
        public void YearlyGivingFullSmokeTest()
        {
            var contactId = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var contacts = new Contact[] { new Contact() { ContactId = contactId, StateCode = 0, Deleted = false } };
            var dbContext = createDbContextMock()
                .WithTransactions(new Transaction[] {
                    new Transaction() { TransactionId = transactionId, Amount = 111, BookDate = DateTime.Today, StatusCode = StatusCode.Completed, TypeCode = TransactionTypeCode.Donation, CustomerId = contactId, CustomerIdType = CustomerIdType.Contact },
                    new Transaction() { TransactionId = Guid.NewGuid(), Amount = 222, BookDate = DateTime.Today, StatusCode = StatusCode.Completed, TypeCode = TransactionTypeCode.Donation, CustomerId = contactId, CustomerIdType = CustomerIdType.Contact },
                })
                .WithContacts(contacts)
                .WithDonorCommitments(new DonorCommitment[0])
                .WithEventPackages(new EventPackage[0])
                .WithAccounts(new Account[0]).Build();
            var synchronizer = new Mock<IDbEntitySynchronizer>();
            var calculator = new YearlyGivingCalculator(dbContext.Object, NullLogger, synchronizer.Object);
            var sut = new YearlyGivingFull(() => dbContext.Object, calculator, NullLoggerFactory);

            sut.FullRecalculation();

            Assert.AreEqual(333, contacts[0].msnfp_year0_giving);
            Assert.AreEqual(333, contacts[0].msnfp_lifetimegivingsum);
            synchronizer.Verify(s => s.SynchronizeEntitiesToDbTransitively(It.IsAny<List<Type>>()), Times.Once());
        }

        private class MockDbContextBuilder
        {
            private Mock<PaymentContext> _mock;

            public MockDbContextBuilder()
            {
                var dbContextMock = new Mock<PaymentContext>();
                dbContextMock.Setup(it => it.Model).Returns(PaymentContextSpy.GetModel());
                _mock = dbContextMock;
            }

            public MockDbContextBuilder WithTransactions(Transaction[] ts)
            {
                _mock.Setup(c => c.Transaction).ReturnsDbSet(ts);
                return this;
            }

            public MockDbContextBuilder WithContacts(Contact[] cs)
            {
                _mock.Setup(c => c.Contact).ReturnsDbSet(cs);
                return this;
            }

            public MockDbContextBuilder WithAccounts(Account[] a)
            {
                _mock.Setup(c => c.Account).ReturnsDbSet(a);
                return this;
            }


            public MockDbContextBuilder WithEventPackages(EventPackage[] es)
            {
                _mock.Setup(c => c.EventPackage).ReturnsDbSet(es);
                return this;
            }

            public MockDbContextBuilder WithDonorCommitments(DonorCommitment[] ds)
            {
                _mock.Setup(c => c.DonorCommitment).ReturnsDbSet(ds);
                return this;
            }

            public Mock<PaymentContext> Build()
            {
                return _mock;
            }
        }

        private static MockDbContextBuilder createDbContextMock()
        {
            return new MockDbContextBuilder();
        }
    }
}