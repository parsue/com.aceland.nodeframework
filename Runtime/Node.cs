using System;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public partial class Node<T> : INode<T>
    {
        public string Id { get; private set; }

        private ParentNode ParentNode { get; set; }
        ParentNode INode.ParentNode => ParentNode;

        private ChildNode ChildNode  { get; set; }
        ChildNode INode.ChildNode => ChildNode;
        
        public T Concrete { get; private set; }

        public void Dispose()
        {
            ParentNode.Dispose();
            ChildNode.Dispose();
            Nodes.Unregister(this);
        }

        publi virtualc void SetParent(INode node)
        {
            ParentNode.SetNode(node);
        }

        public virtual void SetChild(INode node)
        {
            ChildNode.Add(node);
        }

        public virtual void SetChildren(params INode[] nodes)
        {
            ChildNode.Add(nodes);
        }

        public virtual void RemoveChild(INode node)
        {
            ChildNode.Remove(node);
        }

        public virtual void Traverse(Action<INode> action)
        {
            action(this);
            foreach (var node in ChildNode.Nodes)
                node.Traverse(action);
        }
    }
}
