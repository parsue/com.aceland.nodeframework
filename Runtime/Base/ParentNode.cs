using AceLand.Library.Disposable;

namespace AceLand.NodeFramework.Base
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
            Node.ChildNode.Remove(_owner);
            Node = null;
        }

        public INode Node { get; private set; }
        public bool IsRoot => Node == null;

        private readonly INode _owner;

        internal void Set(INode parentNode)
        {
            Node = parentNode;
        }

        internal void SetAsRoot()
        {
            Set(null);
        }
    }
}
