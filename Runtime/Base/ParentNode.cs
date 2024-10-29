using AceLand.Library.Disposable;

namespace AceLand.NodeSystem.Base
{
    public class ParentNode : DisposableObject
    {
        public ParentNode(INode owner) => _owner = owner;

        public ParentNode(INode owner, INode parent)
        {
            _owner = owner;
            _parentNode = parent;
        }

        ~ParentNode() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            _parentNode?.ChildNode?.Remove(_owner);
            _parentNode = null;
        }

        public INode Node => _parentNode;
        public bool IsRoot => _parentNode == null;

        private readonly INode _owner;
        private INode _parentNode;

        internal void SetNode(INode parentNode)
        {
            _parentNode?.ChildNode?.Remove(_owner);
            _parentNode = parentNode;
            _parentNode?.ChildNode?.AddFromParentNode(_owner);
        }

        internal void SetAsRoot()
        {
            SetNode(null);
        }

        internal void SetParentFromChildNode(INode parentNode)
        {
            _parentNode?.ChildNode?.Remove(_owner);
            _parentNode = parentNode;
        }
    }
}
