using System;
using System.Collections.Generic;
using System.Text;
using Data.Utils;
using FluentAssertions;
using FundraisingandEngagement.Models.Entities;
using NUnit.Framework;

namespace Data.Tests.Utils
{
    class DataverseMappingMetadataExtensionsTests
    {
        [Test]
        public void GetsEntityLogicalNameFromAttributes()
        {
            var name = DataverseMappingMetadataExtensions.EntityLogicalName(typeof(Designation));
            name.Should().Be("msnfp_designation");
        }

        [Test]
        public void GetsPropertyLogicalNameFromEntityNameMapAttribute()
        {
            var name = DataverseMappingMetadataExtensions.PropertyLogicalName(typeof(Designation).GetProperty("Name"));
            name.Should().Be("msnfp_name");
        }

        [Test]
        public void GetsPropertyLogicalNameFromEntityOptionSetMapAttribute()
        {
            var name2 = DataverseMappingMetadataExtensions.PropertyLogicalName(typeof(EventSponsor).GetProperty("Order"));
            name2.Should().Be("msnfp_order");
        }

        [Test]
        public void GetsPropertyLogicalNameFromEntityReferenceMapAttribute()
        {
            var name3 = DataverseMappingMetadataExtensions.PropertyLogicalName(typeof(BankRun).GetProperty("PaymentProcessorId"));
            name3.Should().Be("msnfp_paymentprocessorid");
        }
    }
}