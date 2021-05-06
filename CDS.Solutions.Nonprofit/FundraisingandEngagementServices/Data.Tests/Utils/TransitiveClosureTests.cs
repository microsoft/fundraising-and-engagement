using System;
using System.Collections.Generic;
using System.Text;
using Data.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Tests.Utils
{
    class TransitiveClosureTests
    {
        [Test]
        public void ReturnsTransitiveClosure()
        {
            Func<int, IEnumerable<int>> edges = i =>
            {
                switch (i)
                {
                    case 1: return new List<int> { 2 };
                    case 2: return new List<int> { 3 };
                    case 3: return new List<int> { 1 };
                    case 4: return new List<int> { 2, 7 };
                    case 5: return new List<int> { 6 };
                    case 6: return new List<int>();
                    default: throw new InvalidOperationException();
                }
            };
            var result = TransitiveClosure.ReachableNodes(new List<int> { 1, 5 }, edges);

            result.Should().HaveCount(5).And.Contain(new List<int>
            {
                1,
                2,
                3,
                5,
                6
            });
        }
    }
}