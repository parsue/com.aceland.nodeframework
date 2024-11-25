using System;

namespace AceLand.NodeFramework.Core
{
    public interface INode<out T> : INode
        where T : class
    {
        T Concrete { get; }
    }

    public interface INode : IComparable, IComparable<INode>, IEquatable<INode>
    {
        string Id { get; }
        internal ParentNode ParentNode { get; }
        internal ChildNode ChildNode { get; }

        void Dispose();
    }
}
