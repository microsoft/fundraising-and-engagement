using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using NUnit.Framework;

namespace Data.Tests.DataSynchronization
{
    class DbEntitySynchronizerTests
    {
        [Test]
        public void SynchronizesTransitiveEntitiesInReverseTopologicalOrder()
        {
            var dataverseSynchronizerMock = new Mock<IDataverseSynchronizer>();
            var dbContextMock = createDbContextMock();
            var synchronizer = new DbEntitySynchronizer(dataverseSynchronizerMock.Object, dbContextMock.Object);

            // Act
            synchronizer.SynchronizeEntitiesToDbTransitively(new List<Type> { typeof(EventPreference) });

            // Assert
            List<IEnumerable<string>> synchronizedEntitiesCapture = new List<IEnumerable<string>>();
            dataverseSynchronizerMock.Verify(it => it.Synchronize(Capture.In(synchronizedEntitiesCapture)));
            synchronizedEntitiesCapture.Should().ContainSingle().Which.Should()
                .Contain(new List<string> { "msnfp_preferencecategory", "msnfp_eventpreference" }).And
                .HaveElementAt(0, "msnfp_preferencecategory");
        }

        [Test]
        public void SkipsUnmappedEntities()
        {
            var dataverseSynchronizerMock = new Mock<IDataverseSynchronizer>();
            var dbContextMock = createDbContextMock();
            var synchronizer = new DbEntitySynchronizer(dataverseSynchronizerMock.Object, dbContextMock.Object);

            // Act
            synchronizer.SynchronizeEntitiesToDbTransitively(new List<Type> { typeof(TributeOrMemory), typeof(DataverseSyncToken) });

            // Assert
            dataverseSynchronizerMock.Verify(it => it.Synchronize(new List<string> { "msnfp_tributeormemory" }));
        }

        [Test]
        public void IncludesEntitiesReferencingGivenOnesWithForeignKeys()
        {
            var dataverseSynchronizerMock = new Mock<IDataverseSynchronizer>();
            var dbContextMock = createDbContextMock();
            var synchronizer = new DbEntitySynchronizer(dataverseSynchronizerMock.Object, dbContextMock.Object);

            // Act
            synchronizer.SynchronizeEntitiesToDbTransitively(new List<Type> { typeof(PreferenceCategory) });

            // Assert
            List<IEnumerable<string>> synchronizedEntitiesCapture = new List<IEnumerable<string>>();
            dataverseSynchronizerMock.Verify(it => it.Synchronize(Capture.In(synchronizedEntitiesCapture)));
            synchronizedEntitiesCapture.Should().ContainSingle().Which.Should()
                .HaveCount(3).And
                .Contain(new List<string> { "msnfp_preferencecategory", "msnfp_preference", "msnfp_eventpreference" });
        }

        private static Mock<PaymentContext> createDbContextMock()
        {
            var dbContextMock = new Mock<PaymentContext>();
            dbContextMock.Setup(it => it.Model).Returns(PaymentContextSpy.GetModel());
            return dbContextMock;
        }
    }
}