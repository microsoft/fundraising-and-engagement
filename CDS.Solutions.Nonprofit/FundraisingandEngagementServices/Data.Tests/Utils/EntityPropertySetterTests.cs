using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Data.Utils;
using FluentAssertions;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace Data.Tests.Utils
{
    class EntityPropertySetterTests
    {
        [Test]
        public void SetsNullValue()
        {
            var target = new TestObject { StringProp = "abc" };

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.StringProp),
                null
            );

            target.StringProp.Should().BeNull();
        }

        [Test]
        public void SetsNullValueForNullableType()
        {
            var target = new TestObject { IntNullableProp = 1 };

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.IntNullableProp),
                null
            );

            target.IntNullableProp.Should().BeNull();
        }

        [Test]
        public void SetsDefaultWhenSettingNullToValueType()
        {
            var target = new TestObject { IntProp = 1 };

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.IntProp),
                null
            );

            target.IntProp.Should().Be(0);
        }

        [Test]
        public void ConvertsOptionSetValue()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.IntNullableProp),
                new OptionSetValue(42)
            );

            target.IntNullableProp.Should().Be(42);
        }

        [Test]
        public void ConvertsMoney()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.DecimalProp),
                new Money(4.2m)
            );

            target.DecimalProp.Should().Be(4.2m);
        }

        [Test]
        public void ConvertsEntityReferenceToId()
        {
            var target = new TestObject();
            var id = Guid.NewGuid();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.IdProp),
                new EntityReference("msnfp_x", id)
            );

            target.IdProp.Should().Be(id);
        }

        [Test]
        public void ConvertsToEnum()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.EnumProp),
                844060001
            );

            target.EnumProp.Should().Be(PaymentGatewayCode.Stripe);
        }

        [Test]
        public void ConvertsToEnumFromOptionSet()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.EnumProp),
                new OptionSetValue(844060001)
            );

            target.EnumProp.Should().Be(PaymentGatewayCode.Stripe);
        }

        [Test]
        public void ConvertsSimpleDataTypes()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.IntProp),
                (long)-20
            );
            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.StringProp),
                42
            );

            target.IntProp.Should().Be(-20);
            target.StringProp.Should().Be("42");
        }

        [Test]
        public void SetsSameType()
        {
            var target = new TestObject();

            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.OptionSetValueProp),
                new OptionSetValue(42)
            );
            EntityPropertySetter.SetValueWithConversion(
                target,
                GetProperty(it => it.StringProp),
                "val1"
            );

            target.OptionSetValueProp.Should().Be(new OptionSetValue(42));
            target.StringProp.Should().Be("val1");
        }

        [Test]
        public void ThrowsOnIncompatibleTypes()
        {
            var target = new TestObject();
            FluentActions.Invoking(() =>
                EntityPropertySetter.SetValueWithConversion(
                    target,
                    GetProperty(it => it.IntNullableProp),
                    "abc"
                )
            ).Should().Throw<Exception>();
        }

        [Test]
        public void ThrowsOnInvalidGuid()
        {
            var target = new TestObject();

            FluentActions.Invoking(() =>
                EntityPropertySetter.SetValueWithConversion(
                    target,
                    GetProperty(it => it.IdProp),
                    "(╯°□°）╯︵ ┻━┻"
                )
            ).Should().Throw<NotSupportedException>();
        }

        private class TestObject
        {
            public int IntProp { get; set; }
            public int? IntNullableProp { get; set; }
            public string StringProp { get; set; }
            public OptionSetValue OptionSetValueProp { get; set; }
            public decimal DecimalProp { get; set; }
            public Guid IdProp { get; set; }
            public PaymentGatewayCode EnumProp { get; set; }
        }

        private static PropertyInfo GetProperty<TValue>(Expression<Func<TestObject, TValue>> selector)
        {
            // See https://stackoverflow.com/a/491486/
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }

            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}