using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Utils
{
    internal class TransitiveClosure
    {
        /// <summary>
        /// Returns a list of all nodes reachable from <paramref name="initialNodes"/> in reverse topological order, i.e. sinks first.
        /// </summary>
        /// <param name="edgeSupplier">Function that for a given node N returns collection of all nodes M such that an edge (N,M) exists</param>
        public static IEnumerable<T> ReachableNodes<T>(IEnumerable<T> initialNodes, Func<T, IEnumerable<T>> edgeSupplier)
        {
            // See https://en.wikipedia.org/wiki/Depth-first_search
            var stack = new Stack<T>(initialNodes);
            var visitedNodes = new HashSet<T>();
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (!visitedNodes.Contains(node))
                {
                    visitedNodes.Add(node);
                    foreach (var neighbor in edgeSupplier(node))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
            return visitedNodes;
        }
    }
}