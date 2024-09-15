using AceLand.Library.Disposable;
using System;

namespace AceLand.NodeSystem.Base
{
    public class ParentNode : DisposableObject
    {
        public ParentNode(INode owner) => _owner = owner;

        ~ParentNode() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            _parentNode[0]?.ChildNode?.Remove(_owner);
            _parentNode = Array.Empty<INode>();
        }

        public INode Node => _parentNode[0];
        public bool IsRoot => _parentNode[0] == null;

        private readonly INode _owner;
        private INode[] _parentNode = new INode[1];

        internal void SetParent(INode parentNode)
        {
            _parentNode[0]?.ChildNode?.Remove(_owner);
            _parentNode = new INode[1] { parentNode };
            _parentNode[0]?.ChildNode?.AddFromParentNode(_owner);
        }

        internal void SetParentFromChildNode(INode parentNode)
        {
            _parentNode[0]?.ChildNode?.Remove(_owner);
            _parentNode = new INode[1] { parentNode };
        }

        public void Clear()
        {
            _parentNode[0]?.ChildNode?.Remove(_owner);
            _parentNode = new INode[1];
        }
    }
}