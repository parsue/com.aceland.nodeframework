using AceLand.Library.Disposable;
using System.Collections.Generic;

namespace AceLand.NodeSystem.Base
{
    public class ChildNode : DisposableObject
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
            var nodes = _nodes.ToArray();
            
            foreach (var node in nodes)
                node.ParentNode.SetAsRoot();
            
            _nodes.Clear();
        }

        private readonly INode _owner;
        private readonly List<INode> _nodes = new();
        
        public IEnumerable<INode> Nodes => _nodes;
        public int Count => _nodes.Count;
        public bool IsLeaf => _nodes.Count == 0;

        internal void Add(INode node)
        {
            if (_nodes.Contains(node)) return;
            _nodes.Add(node);
            node.ParentNode.SetParentFromChildNode(_owner);
        }

        internal void Add(params INode[] nodes)
        {
            foreach (var node in nodes)
                Add(node);
        }

        internal void AddFromParentNode(INode node)
        {
            if (_nodes.Contains(node)) return;
            _nodes.Add(node);
        }

        internal void AddFromParentNode(params INode[] nodes)
        {
            foreach (var node in nodes)
                AddFromParentNode(node);
        }

        internal void Remove(INode node)
        {
            if (!_nodes.Contains(node)) return;
            _nodes.Remove(node);
        }

        internal void Clear() =>
            _nodes.Clear();
    } 
}
