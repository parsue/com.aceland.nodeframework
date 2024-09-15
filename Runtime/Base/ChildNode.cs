using AceLand.Library.Disposable;
using System.Collections.Generic;

namespace AceLand.NodeSystem.Base
{
    public class ChildNode : DisposableObject
    {
        public ChildNode(INode owner) => _owner = owner;
        ~ChildNode() => Dispose(false);

        protected override void DisposeManagedResources() => DisposeAll();

        private readonly INode _owner;
        private readonly List<INode> _childNodes = new();
        
        public IEnumerable<INode> Nodes => _childNodes;
        public int Count => _childNodes.Count;

        internal void Add(INode node)
        {
            if (_childNodes.Contains(node)) return;
            _childNodes.Add(node);
            node.ParentNode.SetParentFromChildNode(_owner);
        }

        internal void Add(params INode[] nodes)
        {
            foreach (var node in nodes)
            {
                Add(node);
            }
        }

        internal void AddFromParentNode(INode node)
        {
            if (_childNodes.Contains(node)) return;
            _childNodes.Add(node);
        }

        internal void AddFromParentNode(params INode[] nodes)
        {
            foreach (var node in nodes)
            {
                AddFromParentNode(node);
            }
        }

        internal void Remove(INode node)
        {
            if (!_childNodes.Contains(node)) return;
            _childNodes.Remove(node);
        }
        
        private void DisposeAll()
        {
            var nodes = _childNodes.ToArray();
            foreach (var child in nodes)
            {
                child?.Dispose();
            }
            _childNodes.Clear();
        }
    } 
}