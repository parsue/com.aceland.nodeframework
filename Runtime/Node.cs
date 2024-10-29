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

        public void SetParent(INode node)
        {
            ParentNode.SetNode(node);
        }

        public void SetChild(INode node)
        {
            ChildNode.Add(node);
        }

        public void SetChildren(params INode[] nodes)
        {
            ChildNode.Add(nodes);
        }

        public void RemoveChild(INode node)
        {
            ChildNode.Remove(node);
        }

        public void Traverse(Action<INode> action)
        {
            action(this);
            foreach (var node in ChildNode.Nodes)
                node.Traverse(action);
        }
    }
}
