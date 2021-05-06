using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentAssertions;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;

namespace Data.Tests.DataSynchronization
{
    public class DbEntityRepositoryTests
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
        public void InsertsNewEntities()
        {
            // Arrange
            var entity1 = new Entity("msnfp_designation", Guid.NewGuid()) { Attributes = new AttributeCollection { ["msnfp_name"] = "designation1" } };
            var entity2 = new Entity("msnfp_designation", Guid.NewGuid()) { Attributes = new AttributeCollection { ["msnfp_name"] = "designation2" } };

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity1, entity2 });
            }

            // Assert
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                var persistedEntities = context.Designation.OrderBy(it => it.Name).ToList();
                persistedEntities.Should().HaveCount(2);
                persistedEntities[0].DesignationId.Should().Be(entity1.Id);
                persistedEntities[0].Name.Should().Be("designation1");
                persistedEntities[1].DesignationId.Should().Be(entity2.Id);
                persistedEntities[1].Name.Should().Be("designation2");
            }
        }

        [Test]
        public void UpdatesExistingEntity()
        {
            // Arrange
            var existingEntity = new Designation { DesignationId = Guid.NewGuid(), Name = "designation1" };
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Add(existingEntity);
                context.SaveChanges();
            }

            var newName = "designation1";
            var entity = new Entity("msnfp_designation", existingEntity.DesignationId) { Attributes = new AttributeCollection { ["msnfp_name"] = newName } };

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity });
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                var persistedEntity = context.Designation.SingleOrDefault();
                persistedEntity!.DesignationId.Should().Be(existingEntity.DesignationId);
                persistedEntity.Name.Should().Be(newName);
            }
        }

        [Test]
        public void RetainsOnlyEntitiesWithGivenIds()
        {
            // Arrange
            var retained1 = new Designation { DesignationId = Guid.NewGuid(), Name = "designation1" };
            var retained2 = new Designation { DesignationId = Guid.NewGuid(), Name = "designation2" };
            var removed = new Designation { DesignationId = Guid.NewGuid(), Name = "designation3" };
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.AddRange(retained1, retained2, removed);
                context.SaveChanges();
            }

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.RetainOnly("msnfp_designation", new List<Guid> { retained1.DesignationId, retained2.DesignationId });
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Count().Should().Be(2);
                context.Designation.SingleOrDefault(it => it.DesignationId == removed.DesignationId).Should().BeNull();
            }
        }

        [Test]
        public void RetainsOnlyEntitiesWithGivenIdsOverBatchSize()
        {
            // Arrange
            var entities = Enumerable.Range(0, 2 * DbEntityRepository.MaxDeleteBatchSize + 1)
                .Select(it => new Designation { DesignationId = Guid.NewGuid(), Name = "designation" + it });

            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.AddRange(entities);
                context.SaveChanges();
            }

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.RetainOnly("msnfp_designation", new List<Guid>());
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Count().Should().Be(0);
            }
        }

        [Test]
        public void RetainsOnlyEntitiesWithManyGivenIds() // TODO: this should be moved to integration tests to try with actual SQL server
        {
            // See https://stackoverflow.com/questions/8050091/sqlcommand-maximum-parameters-exception-at-2099-parameters
            // Arrange
            var retained1 = new Designation { DesignationId = Guid.NewGuid(), Name = "designation1" };
            var removed = new Designation { DesignationId = Guid.NewGuid(), Name = "designation3" };
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.AddRange(retained1, removed);
                context.SaveChanges();
            }

            List<Guid> toRetain = Enumerable.Range(0, 3000).Select(i => Guid.NewGuid()).ToList();
            toRetain.Add(retained1.DesignationId);

            // Act

            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.RetainOnly("msnfp_designation", toRetain);
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Count().Should().Be(1);
                context.Designation.SingleOrDefault(it => it.DesignationId == removed.DesignationId).Should().BeNull();
            }
        }

        [Test]
        public void DeletesEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Add(new Designation { DesignationId = id, Name = "designation1" });
                context.Designation.Add(new Designation { DesignationId = Guid.NewGuid(), Name = "designation2" });
                context.SaveChanges();
            }

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Delete("msnfp_designation", id);
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Designation.Count().Should().Be(1);
                context.Designation.SingleOrDefault(it => it.DesignationId == id).Should().BeNull();
            }
        }

        [Test]
        public void SucceedsWhenDeletingNonexistentEntityInstance()
        {
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Delete("msnfp_designation", Guid.NewGuid()); // no exception expected
        }

        [Test]
        public void ThrowsWhenDeletingUnknownEntityType()
        {
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Invoking(it => it.Delete("nonexistent", Guid.NewGuid()))
                .Should().Throw<ArgumentException>().WithMessage("Unknown entity 'nonexistent'");
        }

        [Test]
        public void MapsContactPaymentEntityCustomerIdContactType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var entity = new Entity("msnfp_paymentmethod", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_customerid"] = new EntityReference("contact", customerId),
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.PaymentMethod.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.CustomerId.Should().Be(customerId);
            persistedEntity!.CustomerIdType.Should().Be(CustomerIdType.Contact);
        }

        [Test]
        public void MapsContactPaymentEntityCustomerIdAccountType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var entity = new Entity("msnfp_paymentmethod", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_customerid"] = new EntityReference("account", customerId),
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.PaymentMethod.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.CustomerId.Should().Be(customerId);
            persistedEntity!.CustomerIdType.Should().Be(CustomerIdType.Account);
        }

        [Test]
        public void SetsMissingFieldsToNull()
        {
            // Arrange
            var existingEntity = new Contact { ContactId = Guid.NewGuid(), FirstName = "Darth", LastName = "Vader", msnfp_LastTransactionId = Guid.NewGuid() };
            using (var context = new PaymentContext(dbContextOptions))
            {
                context.Contact.Add(existingEntity);
                context.SaveChanges();
            }

            // Note that msnfp_lasttransactionid is missing:
            var entity = new Entity("contact", existingEntity.ContactId) { Attributes = new AttributeCollection { ["firstname"] = "Light" } };

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity });
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                var persistedEntity = context.Contact.SingleOrDefault();
                persistedEntity!.msnfp_LastTransactionId.Should().BeNull();
                persistedEntity.FirstName.Should().Be("Light");
            }
        }

        [Test]
        public void SetsDefaultPaymentEntityProperties()
        {
            // Arrange
            var entity = new Entity("msnfp_designation", Guid.NewGuid()) { Attributes = new AttributeCollection { ["msnfp_name"] = "designation1" } };

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity });
            }

            // Assert
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                var persistedEntity = context.Designation.FirstOrDefault();
                persistedEntity!.Deleted.Should().BeFalse();
                persistedEntity.DeletedDate.Should().BeNull();
                persistedEntity.SyncDate.Should().BeCloseTo(DateTime.Now, 2000);
            }
        }

        [Test]
        public void PreservesLastPaymentAndNextPaymentDate()
        {
            // Arrange
            var id = Guid.NewGuid();
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                context.PaymentSchedule.Add(new PaymentSchedule
                {
                    PaymentScheduleId = id,
                    NextPaymentDate = new DateTime(2020, 1, 30),
                    LastPaymentDate = new DateTime(2020, 1, 20)
                });
                context.SaveChanges();
            }

            // Act
            var entity = new Entity("msnfp_paymentschedule", id) { Attributes = new AttributeCollection { ["msnfp_lastpaymentdate"] = null, ["msnfp_nextpaymentdate"] = null } };
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity });
            }

            // Assert
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                var persistedEntity = context.PaymentSchedule.FirstOrDefault();
                persistedEntity!.NextPaymentDate.Should().NotBeNull();
                persistedEntity.LastPaymentDate.Should().NotBeNull();
            }
        }

        [Test]
        public void PreservesLaterLastPaymentAndNextPaymentDate()
        {
            // Arrange
            var id = Guid.NewGuid();
            var originalLastPaymentDate = new DateTime(2020, 1, 20);
            var originalNextPaymentDate = new DateTime(2020, 1, 30);
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                context.PaymentSchedule.Add(new PaymentSchedule
                {
                    PaymentScheduleId = id,
                    NextPaymentDate = originalNextPaymentDate,
                    LastPaymentDate = originalLastPaymentDate
                });
                context.SaveChanges();
            }

            // Act
            var entity = new Entity("msnfp_paymentschedule", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_lastpaymentdate"] = originalLastPaymentDate.Subtract(TimeSpan.FromDays(1)),
                    ["msnfp_nextpaymentdate"] = originalNextPaymentDate.Subtract(TimeSpan.FromDays(1))
                }
            };
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbEntityRepository(context, NullLogger);
                repository.Upsert(new[] { entity });
            }

            // Assert
            using (var context = new PaymentContext(this.dbContextOptions))
            {
                var persistedEntity = context.PaymentSchedule.FirstOrDefault();
                persistedEntity!.NextPaymentDate.Should().Be(originalNextPaymentDate);
                persistedEntity.LastPaymentDate.Should().Be(originalLastPaymentDate);
            }
        }

        [Test]
        public void CanMapAllPaymentProcessorFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var entity = new Entity("msnfp_paymentprocessor", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_name"] = "name1",
                    ["msnfp_apikey"] = "apiKey1",
                    ["msnfp_identifier"] = "identifier1",
                    ["msnfp_paymentgatewaytype"] = new OptionSetValue(844060000),
                    ["msnfp_storeid"] = "storeId1",
                    ["msnfp_testmode"] = true,
                    ["msnfp_bankrunfileformat"] = new OptionSetValue(844060000),
                    ["msnfp_scotiabankcustomernumber"] = "scotiabanknumber1",
                    ["msnfp_originatorshortname"] = "originator1",
                    ["msnfp_originatorlongname"] = "originator2",
                    ["msnfp_bmooriginatorid"] = "bmo1",
                    ["msnfp_abaremittername"] = "aba1",
                    ["msnfp_abausername"] = "abaUser1",
                    ["msnfp_abausernumber"] = "abaNumber1",
                    ["msnfp_iatsagentcode"] = "iatsCode1",
                    ["msnfp_iatspassword"] = "AURA88",
                    ["createdon"] = now,
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.PaymentProcessor.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.PaymentProcessorId.Should().Be(id);
            persistedEntity.Name.Should().Be("name1");
            persistedEntity.MonerisApiKey.Should().Be("apiKey1");
            persistedEntity.Identifier.Should().Be("identifier1");
            persistedEntity.PaymentGatewayType.Should().Be(PaymentGatewayCode.Moneris);
            persistedEntity.MonerisStoreId.Should().Be("storeId1");
            persistedEntity.MonerisTestMode.Should().Be(true);
            persistedEntity.BankRunFileFormat.Should().Be(844060000);
            persistedEntity.ScotiabankCustomerNumber.Should().Be("scotiabanknumber1");
            persistedEntity.OriginatorShortName.Should().Be("originator1");
            persistedEntity.OriginatorLongName.Should().Be("originator2");
            persistedEntity.BmoOriginatorId.Should().Be("bmo1");
            persistedEntity.AbaRemitterName.Should().Be("aba1");
            persistedEntity.AbaUserName.Should().Be("abaUser1");
            persistedEntity.AbaUserNumber.Should().Be("abaNumber1");
            persistedEntity.IatsAgentCode.Should().Be("iatsCode1");
            persistedEntity.IatsPassword.Should().Be("AURA88");
            persistedEntity.CreatedOn.Should().Be(now);
        }

        [Test]
        public void CanMapAllMembershipCategoryFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var entity = new Entity("msnfp_membershipcategory", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_name"] = "name1",
                    ["msnfp_amount_membership"] = new Money(10.1m),
                    ["msnfp_amount_tax"] = new Money(10.2m),
                    ["msnfp_amount"] = new Money(10.3m),
                    ["msnfp_goodwilldate"] = now,
                    ["msnfp_membershipduration"] = new OptionSetValue(844060000),
                    ["msnfp_renewaldate"] = now,
                    ["transactioncurrencyid"] = new EntityReference("transactioncurrency", currencyId),
                    ["createdon"] = now,
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.MembershipCategory.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.MembershipCategoryId.Should().Be(id);
            persistedEntity.Name.Should().Be("name1");
            persistedEntity.AmountMembership.Should().Be(10.1m);
            persistedEntity.AmountTax.Should().Be(10.2m);
            persistedEntity.Amount.Should().Be(10.3m);
            persistedEntity.GoodWillDate.Should().Be(now);
            persistedEntity.MembershipDuration.Should().Be(844060000);
            persistedEntity.RenewalDate.Should().Be(now);
            persistedEntity.TransactionCurrencyId.Should().Be(currencyId);
            persistedEntity.CreatedOn.Should().Be(now);
        }

        [Test]
        public void CanMapContactFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var entity = new Entity("contact", id)
            {
                Attributes = new AttributeCollection
                {
                    ["address1_addressid"] = Guid.NewGuid(),
                    ["address1_addresstypecode"] = new OptionSetValue(844060000),
                    ["address1_city"] = "city1",
                    ["address1_latitude"] = 1.0f,
                    ["msnfp_birthday"] = new DateTime(2020, 1, 31),
                    ["donotbulkemail"] = true,
                    ["emailaddress1"] = "mail@example.com",
                    ["firstname"] = "first",
                    ["lastname"] = "last",
                    ["fullname"] = "full",
                    ["gendercode"] = new OptionSetValue(844060000),
                    ["masterid"] = Guid.NewGuid(),
                    ["owningbusinessunit"] = new EntityReference("businessunit", Guid.NewGuid()),
                    ["msnfp_age"] = 32,
                    ["msnfp_anonymity"] = new OptionSetValue(844060000),
                    ["msnfp_givinglevelid"] = new EntityReference("msnfp_givinglevelinstance", Guid.NewGuid()),
                    ["msnfp_lasttransactiondate"] = now,
                    ["msnfp_lasttransactionid"] = Guid.NewGuid(),
                    ["msnfp_count_lifetimetransactions"] = 22,
                    ["msnfp_sum_lifetimetransactions"] = new Money(22m).Value,
                    ["parentcustomerid"] = Guid.NewGuid(),
                    ["transactioncurrencyid"] = new EntityReference("transactioncurrency", currencyId),
                    ["statecode"] = new OptionSetValue(844060000),
                    ["statuscode"] = new OptionSetValue(1),
                    ["msnfp_year0_giving"] = new Money(23m).Value,
                    // ... more boring fields
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.Contact.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.Address1_AddressId.Should().NotBeEmpty();
            persistedEntity.Address1_City.Should().Be("city1");
            persistedEntity.Address1_Latitude.Should().Be(1.0f);
            persistedEntity.BirthDate.Should().Be(new DateTime(2020, 1, 31));
            persistedEntity.DoNotBulkEMail.Should().Be(true);
            persistedEntity.EmailAddress1.Should().Be("mail@example.com");
            persistedEntity.FirstName.Should().Be("first");
            persistedEntity.LastName.Should().Be("last");
            persistedEntity.FullName.Should().Be("full");
            persistedEntity.GenderCode.Should().Be(844060000);
            persistedEntity.MasterId.Should().NotBeNull();
            persistedEntity.OwningBusinessUnitId.Should().NotBeNull();
            persistedEntity.msnfp_Age.Should().Be(32);
            persistedEntity.msnfp_Anonymity.Should().Be(844060000);
            persistedEntity.msnfp_Count_LifetimeTransactions.Should().Be(22);
            persistedEntity.msnfp_GivingLevelId.Should().NotBeNull();
            persistedEntity.msnfp_LastTransactionDate.Should().Be(now);
            persistedEntity.msnfp_LastTransactionId.Should().NotBeNull();
            persistedEntity.msnfp_Sum_LifetimeTransactions.Should().Be(22m);
            persistedEntity.ParentCustomerId.Should().NotBeNull();
            persistedEntity.TransactionCurrencyId.Should().Be(currencyId);
            persistedEntity.StateCode.Should().Be(844060000);
            persistedEntity.StatusCode.Should().Be(StatusCode.Active);
            persistedEntity.msnfp_year0_giving.Should().Be(23m);
        }

        [Test]
        public void CanMapAllPaymentScheduleFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var entity = new Entity("msnfp_paymentschedule", id)
            {
                Attributes = new AttributeCollection
                {
                    ["msnfp_amount_receipted"] = new Money(10m),
                    ["msnfp_amount_membership"] = new Money(11m),
                    ["msnfp_amount_nonreceiptable"] = new Money(12m),
                    ["msnfp_amount_tax"] = new Money(13m),
                    ["msnfp_recurringamount"] = new Money(14m),
                    ["msnfp_firstpaymentdate"] = now,
                    ["msnfp_frequencyinterval"] = 1,
                    ["msnfp_frequency"] = new OptionSetValue(844060000),
                    ["msnfp_nextpaymentdate"] = new DateTime(2100, 1, 1),
                    ["msnfp_frequencystartcode"] = new OptionSetValue(844060001),
                    ["msnfp_cancelationcode"] = new OptionSetValue(844060002),
                    ["msnfp_cancelledon"] = new DateTime(2100, 1, 2),
                    ["msnfp_endondate"] = new DateTime(2100, 1, 3),
                    ["msnfp_lastpaymentdate"] = new DateTime(2100, 1, 4),
                    ["msnfp_scheduletypecode"] = new OptionSetValue(844060003),
                    ["msnfp_anonymity"] = new OptionSetValue(844060004),
                    ["msnfp_paymentmethodid"] = new EntityReference("msnfp_paymentmethodid", Guid.NewGuid()),
                    ["msnfp_designationid"] = new EntityReference("msnfp_designationid", Guid.NewGuid()),
                    // ...
                    ["msnfp_billing_stateorprovince"] = "mechalondon",
                    ["msnfp_ccbrandcode"] = new OptionSetValue(844060001),
                    ["msnfp_chargeoncreate"] = true,
                    ["msnfp_configurationid"] = new EntityReference("msnfp_configuration", Guid.NewGuid()),
                    ["msnfp_customerid"] = new EntityReference("account", Guid.NewGuid()),
                    ["msnfp_paymentprocessorid"] = new EntityReference("msnfp_paymentprocessor", Guid.NewGuid()),
                    // ...
                    ["msnfp_firstname"] = "first",
                    ["transactioncurrencyid"] = new EntityReference("transactioncurrency", Guid.NewGuid()),
                }
            };

            // Act
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbEntityRepository(context, NullLogger);
            repository.Upsert(new[] { entity });

            // Assert
            var persistedEntity = context.PaymentSchedule.SingleOrDefault();
            persistedEntity.Should().NotBeNull();
            persistedEntity!.PaymentScheduleId.Should().Be(id);
            persistedEntity!.AmountReceipted.Should().Be(10m);
            persistedEntity!.AmountMembership.Should().Be(11m);
            persistedEntity!.AmountNonReceiptable.Should().Be(12m);
            persistedEntity!.AmountTax.Should().Be(13m);
            persistedEntity!.RecurringAmount.Should().Be(14m);
            persistedEntity!.FirstPaymentDate.Should().Be(now);
            persistedEntity!.FrequencyInterval.Should().Be(1);
            persistedEntity!.Frequency.Should().Be(FrequencyType.Days);
            persistedEntity!.NextPaymentDate.Should().Be(new DateTime(2100, 1, 1));
            persistedEntity!.FrequencyStartCode.Should().Be(FrequencyStart.FirstOfMonth);
            persistedEntity!.CancelationCode.Should().Be(844060002);
            persistedEntity!.CancelledOn.Should().Be(new DateTime(2100, 1, 2));
            persistedEntity!.EndonDate.Should().Be(new DateTime(2100, 1, 3));
            persistedEntity!.LastPaymentDate.Should().Be(new DateTime(2100, 1, 4));
            persistedEntity!.ScheduleTypeCode.Should().Be(844060003);
            persistedEntity!.Anonymity.Should().Be(844060004);
            persistedEntity!.PaymentMethodId.Should().NotBeNull();
            persistedEntity!.DesignationId.Should().NotBeNull();
            persistedEntity!.BillingStateorProvince.Should().Be("mechalondon");
            persistedEntity!.CcBrandCode.Should().Be(844060001);
            persistedEntity!.ChargeonCreate.Should().Be(true);
            persistedEntity!.ConfigurationId.Should().NotBeNull();
            persistedEntity!.CustomerId.Should().NotBeNull();
            persistedEntity!.CustomerIdType.Should().Be(CustomerIdType.Account);
            persistedEntity!.PaymentProcessorId.Should().NotBeNull();
            persistedEntity!.FirstName.Should().Be("first");
            persistedEntity!.TransactionCurrencyId.Should().NotBeNull();
        }
    }
}