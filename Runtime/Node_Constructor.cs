using System;
using AceLand.Library.Disposable;
using AceLand.Library.Optional;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public partial class Node<T> : DisposableObject
    {
        protected internal Node(Option<string> id, INode parentNode, INode[] childNodes, T concrete)
        {
            Id = id.Reduce($"{nameof(T)}_{Guid.NewGuid()}");
            ParentNode = parentNode is null
                ? new ParentNode(this)
                : new ParentNode(this, parentNode);
            ChildNode = childNodes.Length == 0
                ? new ChildNode(this)
                : new ChildNode(this, childNodes);
            Concrete = concrete;
            
            Nodes.Register(this);
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
