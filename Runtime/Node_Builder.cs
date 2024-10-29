using System.Collections.Generic;
using AceLand.LocalTools.Optional;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public partial class Node<T>
    {
        public static INodeBuilderConcreteObject Builder() =>
            new NodeBuilder();

        public interface INodeBuilderConcreteObject
        {
            INodeBuilder WithConcreteObject(T concrete);
        }

        public interface INodeBuilder
        {
            INode<T> Build();
            INodeBuilder WithId(string id);
            INodeBuilder WithParent(INode node);
            INodeBuilder WithChild(INode node);
            INodeBuilder WithChildren(params INode[] node);
        }
        
        public class NodeBuilder : INodeBuilder, INodeBuilderConcreteObject
        {
            private string _id = string.Empty;
            private INode _parentNode;
            private readonly List<INode> _childNode = new();
            private T _concrete;

            public INode<T> Build() =>
                new Node<T>(_id.ToOption(), _parentNode, _childNode.ToArray(), _concrete);

            public INodeBuilder WithConcreteObject(T obj)
            {
                _concrete = obj;
                return this;
            }
            
            public INodeBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public INodeBuilder WithParent(INode node)
            {
                _parentNode = node;
                return this;
            }

            public INodeBuilder WithChild(INode node)
            {
                _childNode.Add(node);
                return this;
            }

            public INodeBuilder WithChildren(params INode[] nodes)
            {
                _childNode.AddRange(nodes);
                return this;
            }
        }
    }
}
