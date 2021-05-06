using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Utils
{
    internal class TopologicalSort<T>
    {
        private readonly Func<T, IEnumerable<T>> edgeSupplier;
        private readonly Func<T, string> nodeToString;

        /// <param name="edgeSupplier">Function that for a given node N returns collection of all nodes M such that an edge (N,M) exists</param>
        /// <param name="nodeToString">Function that returns a textual description of a node (used for error messages)</param>
        public TopologicalSort(Func<T, IEnumerable<T>> edgeSupplier, Func<T, string> nodeToString)
        {
            this.nodeToString = nodeToString;
            this.edgeSupplier = edgeSupplier;
        }


        /// <summary>
        /// Returns a list of all nodes reachable from <paramref name="initialNodes"/> in reverse topological order, i.e. sinks first.
        /// </summary>
        public IList<T> ReverseTopologicalSort(IEnumerable<T> initialNodes)
        {
            // See https://en.wikipedia.org/wiki/Topological_sorting#Depth-first_search
            var openNodes = new HashSet<T>();
            var visitedNodes = new HashSet<T>();
            var sortedNodes = new List<T>();
            foreach (var node in initialNodes)
            {
                if (node == null)
                {
                    throw new NullReferenceException($"Null node given in {nameof(initialNodes)}");
                }

                if (!visitedNodes.Contains(node))
                {
                    visit(node, openNodes, visitedNodes, sortedNodes);
                }
            }

            return sortedNodes;
        }

        private void visit(T node, ISet<T> openNodes, ISet<T> visitedNodes, List<T> sortedNodes)
        {
            if (visitedNodes.Contains(node))
            {
                return;
            }

            if (openNodes.Contains(node))
            {
                throw new InvalidOperationException($"Cycle containing {nodeToString(node)} detected: {String.Join(", ", openNodes.Select(nodeToString))}");
            }

            openNodes.Add(node);
            foreach (var nextNode in edgeSupplier(node))
            {
                if (nextNode == null)
                {
                    throw new NullReferenceException($"Null node encountered when traversing {nodeToString(node)} edges");
                }

                visit(nextNode, openNodes, visitedNodes, sortedNodes);
            }

            openNodes.Remove(node);
            visitedNodes.Add(node);
            sortedNodes.Add(node);
        }
    }
}