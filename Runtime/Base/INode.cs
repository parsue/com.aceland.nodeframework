using System;
using UnityEngine;

namespace AceLand.NodeSystem.Base
{
    public interface INode<T> : INode
    {
        T Concrete { get; }
    }

    public interface INode
    {
        string Id { get; }
        ParentNode ParentNode { get; }
        ChildNode ChildNode { get; }

        void Dispose();

        void SetParent(INode node);

        void SetChild(INode node);
        void SetChildren(params INode[] nodes);
        void RemoveChild(INode node);

        void Traverse(Action<INode> action);
    }
}
