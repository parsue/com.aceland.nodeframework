using System;
using AceLand.Library.Disposable;
using AceLand.Library.Optional;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework
{
    public partial class Node<T> : DisposableObject
    {
        protected Node(Option<string> id, INode parentNode, INode[] childNodes)
        {
            Id = id.Reduce($"{nameof(T)}_{Guid.NewGuid()}");
            
            ParentNode = parentNode is null
                ? new ParentNode(this)
                : new ParentNode(this, parentNode);
            
            ChildNode = ChildNode is null || childNodes.Length == 0
                ? new ChildNode(this)
                : new ChildNode(this, childNodes);
            
            Concrete = this as T;
            Nodes.Register(Concrete);
        }
        
        internal Node(Option<string> id, INode parentNode, INode[] childNodes, T concrete)
        {
            Id = id.Reduce($"{nameof(T)}_{Guid.NewGuid()}");
            ParentNode = parentNode is null
                ? new ParentNode(this)
                : new ParentNode(this, parentNode);
            ChildNode = ChildNode is null || childNodes.Length == 0
                ? new ChildNode(this)
                : new ChildNode(this, childNodes);
            Concrete = concrete;
            
            Nodes.Register(concrete);
        }

        ~Node() => Dispose(false);

        protected override void DisposeManagedResources()
        {
            ParentNode.Dispose();
            ChildNode.Dispose();
            Nodes.Unregister(this);
        }
    }
}
