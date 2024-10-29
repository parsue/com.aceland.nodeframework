using System;
using System.Collections.Generic;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public static class NodeExtensions
    {
        public static bool IsRoot(this INode node) =>
            node.ParentNode.IsRoot;

        public static bool IsLeaf(this INode node) =>
            node.ChildNode.IsLeaf;
        
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

        public static void SetParent(this INode node, INode parentNode) =>
            node.ParentNode.SetNode(parentNode);
        
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

        public static void AddChild(this INode node, INode childNode) =>
            node.ChildNode.Add(childNode);

        public static void AddChildren(this INode node, params INode[] childNodes) =>
            node.ChildNode.Add(childNodes);

        public static void RemoveChild(this INode node, INode childNode) =>
            node.ChildNode.Remove(childNode);
        
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

        public static IEnumerable<T> Children<T>(this INode node) where T : INode
        {
            foreach (var n in node.ChildNode.Nodes)
            {
                if (n is not T child) continue;
                yield return child;
            }
        }

        public static List<T> ChildrenInAllLevel<T>(this INode node) where T : INode
        {
            var children = new List<T>();

            node.Traverse(n =>
            {
                if (n is not T child) return;
                children.Add(child);
            });
            
            return children;
        }

        public static void Traverse(this INode node, Action<INode> action)
        {
            action(node);
            foreach (var n in node.ChildNode.Nodes)
                n.Traverse(action);
        }
    }
}
