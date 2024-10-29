using System;
using AceLand.LocalTools.Optional;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public partial class Node<T>
    {
        internal Node(Option<string> id, INode parentNode, INode[] childNodes, T concrete)
        {
            Initialize(id, parentNode, childNodes, concrete);
        }

        internal void Initialize(Option<string> id, INode parentNode, INode[] childNodes, T concrete)
        {
            Id = id.Reduce(Guid.NewGuid().ToString);
            ParentNode = parentNode is null
                ? new ParentNode(this)
                : new ParentNode(this, parentNode);
            ChildNode = childNodes.Length == 0
                ? new ChildNode(this)
                : new ChildNode(this, childNodes);
            Concrete = concrete;
            
            Nodes.Register(this);
        }
    }
}
