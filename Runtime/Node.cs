using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public partial class Node<T> : INode<T>
        where T : class
    {
        public string Id { get; }

        private ParentNode ParentNode { get; }
        ParentNode INode.ParentNode => ParentNode;

        private ChildNode ChildNode  { get; }
        ChildNode INode.ChildNode => ChildNode;
        
        public T Concrete { get; }
    }
}
