using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework
{
    public partial class Node<T> : INode<T>
        where T : class
    {
        public string Id { get; }

        internal ParentNode ParentNode { get; }
        ParentNode INode.ParentNode => ParentNode;

        internal ChildNode ChildNode  { get; }
        ChildNode INode.ChildNode => ChildNode;
        
        public T Concrete { get; }
    }
}
