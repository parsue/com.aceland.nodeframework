using AceLand.Library.Disposable;

namespace AceLand.NodeFramework.Core
{
    internal class ParentNode : DisposableObject
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

        public void Set(INode parentNode)
        {
            Node = parentNode;
        }

        public void SetAsRoot()
        {
            Set(null);
        }
    }
}
