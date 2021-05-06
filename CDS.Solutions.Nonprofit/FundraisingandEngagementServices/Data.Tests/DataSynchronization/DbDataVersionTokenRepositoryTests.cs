
using System;
using System.Collections.Generic;
using System.Linq;
using Data.DataSynchronization;
using FluentAssertions;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;

namespace Data.Tests.DataSynchronization
{
    public class DbDataVersionTokenRepositoryTests
    {
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
        public void InsertsNewToken()
        {
            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Put("msnfp_designation", "token1");
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                var persistedEntity = context.DataverseSyncToken.SingleOrDefault();
                persistedEntity.Should().NotBeNull();
                persistedEntity!.TokenValue.Should().Be("token1");
                persistedEntity.EntityLogicalName.Should().Be("msnfp_designation");
            }
        }

        [Test]
        public void GetsExistingToken()
        {
            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Put("msnfp_configuration", "token1");
                repository.Put("msnfp_designation", "token2");
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Get("msnfp_configuration").Should().Be("token1");
                repository.Get("msnfp_designation").Should().Be("token2");
            }
        }



        [Test]
        public void ReturnsNullIfTokenUnknown()
        {
            // Assert
            using var context = new PaymentContext(this.dbContextOptions);
            var repository = new DbDataVersionTokenRepository(context);
            repository.Get("msnfp_configuration").Should().BeNull();
        }


        [Test]
        public void UpdatesExistingToken()
        {
            // Arrange
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Put("msnfp_configuration", "token1");
            }

            // Act
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Put("msnfp_configuration", "token1 updated");
            }

            // Assert
            using (var context = new PaymentContext(dbContextOptions))
            {
                var repository = new DbDataVersionTokenRepository(context);
                repository.Get("msnfp_configuration").Should().Be("token1 updated");
            }
        }
    }
}