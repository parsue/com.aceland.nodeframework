using System;

namespace AceLand.NodeSystem.Base
{
    public interface INode<out T> : INode
        where T : class
    {
        T Concrete { get; }
    }

    public interface INode
    {
        string Id { get; }
        internal ParentNode ParentNode { get; }
        internal ChildNode ChildNode { get; }

        void Dispose();
    }
}
