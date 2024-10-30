using System.Collections.Generic;
using AceLand.Library.Disposable;

namespace AceLand.NodeFramework.Base
{
    internal class ChildNode : DisposableObject
    {
        public ChildNode(INode owner) => _owner = owner;

        public ChildNode(INode owner, INode[] children)
        {
            _owner = owner;
            _nodes.AddRange(children);
        }
        
        ~ChildNode() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            foreach (var node in _nodes)
                node.ParentNode.SetAsRoot();

            Clear();
        }

        private readonly INode _owner;
        private readonly List<INode> _nodes = new();
        
        public IEnumerable<INode> Nodes => _nodes;
        public int Count => _nodes.Count;
        public bool IsLeaf => _nodes.Count == 0;

        internal bool Contains(INode node) =>
            _nodes.Contains(node);

        internal void Add(INode node)
        {
            if (_nodes.Contains(node)) return;
            _nodes.Add(node);
        }

        internal void Add(params INode[] nodes)
        {
            foreach (var node in nodes)
                Add(node);
        }

        internal void Remove(INode node)
        {
            if (!_nodes.Contains(node)) return;
            _nodes.Remove(node);
        }

        internal void Clear()
        {
            _nodes.Clear();
        }
    } 
}
