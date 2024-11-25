using System;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework
{
    public partial class Node<T> : INode<T>
        where T : class, INode
    {
        public string Id { get; }

        private ParentNode ParentNode { get; }
        ParentNode INode.ParentNode => ParentNode;

        private ChildNode ChildNode  { get; }
        ChildNode INode.ChildNode => ChildNode;
        
        public T Concrete { get; }
        
        public bool Equals(INode x, INode y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }
        public bool Equals(INode other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        // Override Object.Equals
        public override bool Equals(object obj)
        {
            if (obj is INode other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // IComparable<MyClass>
        public int CompareTo(INode other)
        {
            if (other == null) return 1;
            return string.Compare(Id, other.Id); 
        }

        // IComparable
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is INode other)
                return CompareTo(other);

            throw new ArgumentException("Object is not a Node");
        }
    }
}
