using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace ChessDotNet.AI.Search
{
    // This priority queue is for working with chess positions scored using a simple scalar (int) value.
    // Each discrete priority level can have any number of search tree nodes, 
    // all of which are considered equivalent priotity to further investigate.
    public class TreeNodePriorityQueue : ICollection<SearchTreeNode>
    {
        SortedDictionary<int, List<SearchTreeNode>> PriorityQueue;

        public TreeNodePriorityQueue()
        {
            PriorityQueue = new SortedDictionary<int, List<SearchTreeNode>>();
        }

        public IEnumerable<SearchTreeNode> AllNodes
            => PriorityQueue.SelectMany(kvp => kvp.Value);

        public int Count => AllNodes.Count();

        public bool IsReadOnly => false;

        public void Clear() => PriorityQueue.Clear();

        public bool Contains(SearchTreeNode node)
            => PriorityQueue.ContainsKey(node.Score) && PriorityQueue[node.Score].Contains(node);

        public void CopyTo(SearchTreeNode[] array, int index)
            => AllNodes.ToArray().CopyTo(array, index);

        public void Add(SearchTreeNode node)
        {
            // add priority level if it doesn't already exist
            if (!PriorityQueue.ContainsKey(node.Score))
                PriorityQueue.Add(node.Score, new List<SearchTreeNode>());

            // add the new node to the correct prioritized level of the queue
            PriorityQueue[node.Score].Add(node);

            // listen for score changes
            node.PropertyChanged += Node_PropertyChanged;
        }

        bool Remove(SearchTreeNode node, int score)
        {
            // if the priority level isn't registered or the node can't be found at that priority level
            if (!PriorityQueue.ContainsKey(score) || !PriorityQueue[score].Contains(node))
                return false; // can't remove

            // try to remove and return success or failure
            var success = PriorityQueue[score].Remove(node);

            // remove the event handler for the removed node
            node.PropertyChanged -= Node_PropertyChanged;

            // if removing this node led to the priority level being empty, remove it (clean up)
            if (PriorityQueue[score].Count == 0)
                PriorityQueue.Remove(score);

            return success;
        }

        public bool Remove(SearchTreeNode node) => Remove(node, node.Score);

        private void Node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var node = (SearchTreeNode)sender;

            // when a node's score changes, update it's position in the prioritized queue
            if (e.PropertyName == nameof(SearchTreeNode.Score))
            {
                Remove(node, node.PreviousScore);
                Add(node);
            }
        }

        #region Enumerator

        public IEnumerator<SearchTreeNode> GetEnumerator()
        {
            foreach (var node in AllNodes.OrderBy(n => n.Score))
                yield return node;

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
