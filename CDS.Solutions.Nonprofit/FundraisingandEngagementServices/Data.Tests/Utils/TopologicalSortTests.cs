using System;
using System.Collections.Generic;
using System.Text;
using Data.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Tests.Utils
{
    class TopologicalSortTests
    {
        [Test]
        public void ReturnsReverseTopologicalOrder()
        {
            Func<int, IEnumerable<int>> edges = i =>
            {
                switch (i)
                {
                    case 1: return new List<int> { 2, 3 };
                    case 2: return new List<int> { 3 };
                    case 3: return new List<int>();
                    default: throw new InvalidOperationException();
                }
            };
            var sut = new TopologicalSort<int>(edges, i => i.ToString());
            var result = sut.ReverseTopologicalSort(new List<int> { 3, 1, 2 });

            result.Should().ContainInOrder(new List<int> { 3, 2, 1 });
        }

        [Test]
        public void ReturnsAllReachableNodes()
        {
            Func<int, IEnumerable<int>> edges = i =>
            {
                switch (i)
                {
                    case 1: return new List<int> { 2, 3 };
                    case 2: return new List<int> { 3 };
                    case 3: return new List<int>();
                    default: throw new InvalidOperationException();
                }
            };
            var sut = new TopologicalSort<int>(edges, i => i.ToString());
            var result = sut.ReverseTopologicalSort(new List<int> { 1 });

            result.Should().BeEquivalentTo(new List<int> { 3, 2, 1 });
        }

        [Test]
        public void DetectsCycle()
        {
            Func<int, IEnumerable<int>> edges = i =>
            {
                switch (i)
                {
                    case 1: return new List<int> { 2, 3 };
                    case 2: return new List<int> { 3 };
                    case 3: return new List<int> { 1 };
                    default: throw new InvalidOperationException();
                }
            };
            var sut = new TopologicalSort<int>(edges, i => i.ToString());
            sut.Invoking(it => it.ReverseTopologicalSort(new List<int> { 1 }))
                .Should().Throw<InvalidOperationException>().WithMessage("Cycle containing 1*");
        }
    }
}