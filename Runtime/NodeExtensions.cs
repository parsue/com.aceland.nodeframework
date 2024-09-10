using System;
using System.Collections.Generic;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public static class NodeExtensions
    {
        public static T Root<T>(this INode node) where T : INode
        {
            var parent = node.ParentNode.Node;
            if (parent != null)
            {
                while (!parent.ParentNode.IsRoot)
                    parent = parent.ParentNode.Node;
            }
            else
            {
                parent = node;
            }

            if (parent is not T parentNode)
                throw new Exception($"wrong type of {nameof(T)}");

            return parentNode;
        }
        
        public static T Parent<T>(this INode node) where T : INode
        {
            if (node.ParentNode.Node is not T parent)
                throw new Exception($"wrong type of {nameof(T)}");

            return parent;
        }
        
        public static T Child<T>(this INode node) where T : INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode is not T n) continue;
                return n;
            }
            throw new Exception($"ChildNode [{nameof(T)}] not find");
        }

        public static T Child<T>(this INode node, string id) where T : INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is not T n) continue;
                return n;
            }
            throw new Exception($"ChildNode [{id}] not find");
        }

        public static T Neighbour<T>(this INode node) where T : INode
        {
            foreach (var childNode in node.ParentNode.Node.ChildNode.Nodes)
            {
                if (childNode is not T n) continue;
                return n;
            }
            throw new Exception($"ChildNode [{nameof(T)}] not find");
        }

        public static T Neighbour<T>(this INode node, string id) where T : INode
        {
            foreach (var childNode in node.ParentNode.Node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is not T n) continue;
                return n;
            }
            throw new Exception($"ChildNode [{id}] not find");
        }

        public static IEnumerable<T> Childs<T>(this INode node) where T : INode
        {
            foreach (var n in node.ChildNode.Nodes)
            {
                if (n is not T child) continue;
                yield return child;
            }
        }

        public static List<T> Children<T>(this INode node) where T : INode
        {
            var children = new List<T>();
            
            foreach (var childNOde in node.ChildNode.Nodes)
            {
                childNOde.Traverse(n =>
                {
                    if (n is not T child) return;
                    children.Add(child);
                });
            }
            
            return children;
        }
    }
}
