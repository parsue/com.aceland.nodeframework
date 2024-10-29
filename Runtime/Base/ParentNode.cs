using AceLand.Library.Disposable;

namespace AceLand.NodeSystem.Base
{
    public class ParentNode : DisposableObject
    {
        public ParentNode(INode owner) => _owner = owner;

        public ParentNode(INode owner, INode parent)
        {
            _owner = owner;
            Node = parent;
        }

        ~ParentNode() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            Node?.ChildNode?.Remove(_owner);
            Node = null;
        }

        public INode Node { get; private set; }
        public bool IsRoot => Node == null;

        private readonly INode _owner;

        internal void SetNode(INode parentNode)
        {
            Node?.ChildNode?.Remove(_owner);
            Node = parentNode;
            Node?.ChildNode?.AddFromParentNode(_owner);
        }

        internal void SetAsRoot()
        {
            SetNode(null);
        }

        internal void SetParentFromChildNode(INode parentNode)
        {
            Node?.ChildNode?.Remove(_owner);
            Node = parentNode;
        }
    }
}
