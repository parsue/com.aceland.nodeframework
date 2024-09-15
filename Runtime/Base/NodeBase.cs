using AceLand.Library.Disposable;
using AceLand.Library.Extensions;
using System;

namespace AceLand.NodeSystem.Base
{
    public abstract class NodeBase<T> : DisposableObject, INode<T>
        where T : NodeBase<T>
    {
        public NodeBase(string id = "", INode parentNode = null, params INode[] childNodes)
        {
            Id = id.IsNullOrEmptyOrWhiteSpace() ? $"{nameof(T)}_{Guid.NewGuid()}" : id;
            ParentNode = new(this);
            ChildNode = new(this);
            SetParent(parentNode);
            SetChildren(childNodes);
            Concrete = (T)this;
            Nodes.Register(Concrete);
        }

        ~NodeBase() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            Nodes.Unregister(Concrete);
            ParentNode.Dispose();
            ChildNode.Dispose();
        }

        public string Id { get; protected set; }
        public T Concrete { get; protected set; }

        public INode Parent => ParentNode.Node;
        public ParentNode ParentNode { get; set; }
        public ChildNode ChildNode { get; set; }

        public void SetParent(INode node) => ParentNode.SetParent(node);

        public void SetChild(INode node) => ChildNode.Add(node);
        public void SetChildren(params INode[] nodes) => ChildNode.Add(nodes);
        public void RemoveChild(INode node) => ChildNode.Remove(node);
        
        public virtual void Traverse(Action<T> action)
        {
            action(Concrete);
            foreach (var childNode in ChildNode.Nodes)
            {
                var tNode = (T)childNode;
                tNode.Traverse(action);
            }
        }
        
        public virtual void Traverse(Action<INode> action)
        {
            action(this);
            foreach (var childNode in ChildNode.Nodes)
                childNode.Traverse(action);
        }
    }
}