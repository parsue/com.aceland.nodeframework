using System;
using System.Collections.Generic;
using System.Linq;
using AceLand.NodeFramework.Core;

namespace AceLand.NodeFramework
{
    public static class NodeExtensions
    {
        public static void Register<T>(this T node) where T : INode =>
            Nodes.Register(node);
        
        public static void Unregister<T>(this T node) where T : INode =>
            Nodes.Unregister(node);
        
        public static bool IsRoot(this INode node) =>
            node.ParentNode?.IsRoot ?? true;

        public static bool IsLeaf(this INode node) =>
            node.ChildNode?.IsLeaf ?? true;
        
        public static INode Root(this INode node)
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

            return parent;
        }
        
        public static T Root<T>(this INode node) where T : class
        {
            var parent = node.Root();
        
            if (parent is T parentNode)
                return parentNode;
            
            throw new Exception($"wrong type of {typeof(T).Name}");
        }
        
        public static INode Parent(this INode node) =>
            node.ParentNode.Node;
        
        public static T Parent<T>(this INode node) where T : class
        {
            if (node.ParentNode.Node is T parent)
                return parent;
            
            throw new Exception($"wrong type of {typeof(T).Name}");
        }

        public static void SetParent(this INode node, INode parentNode)
        {
            if (parentNode is null) return;
            
            if (!node.IsRoot())
                node.Parent().ChildNode.Remove(node);
            
            parentNode.ChildNode?.Add(node);
            node.ParentNode?.Set(parentNode);
        }

        public static void SetAsRoot(this INode node)
        {
            if (!node.IsRoot())
                node.Parent().ChildNode.Remove(node);
            
            node.ParentNode.SetAsRoot();
        }
        
        public static INode Child(this INode node) =>
            node.ChildNode.Nodes.First();

        public static T Child<T>(this INode node) where T : class
        {
            foreach (var childNode in node.ChildNode.Nodes)
                if (childNode is T n) return n;
            
            throw new Exception($"ChildNode [{typeof(T).Name}] not find");
        }

        public static INode Child(this INode node, string id)
        {
            foreach (var childNode in node.ChildNode.Nodes)
                if (childNode.Id == id) return childNode;
            
            throw new Exception($"ChildNode [{id}] not find");
        }

        public static T Child<T>(this INode node, string id) where T : class
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is T n) return n;
            }
            
            throw new Exception($"ChildNode [{id}] of [{nameof(T)}] not find");
        }
        
        public static IEnumerable<INode> Children(this INode node) =>
            node.ChildNode.Nodes;

        public static IEnumerable<T> Children<T>(this INode node) where T : class
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode is T n) yield return n;
            }
        }

        public static IEnumerable<INode> Children(this INode node, string id)
        {
            var nodes = new List<INode>();
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                nodes.Add(childNode);
            }

            return nodes;
        }

        public static IEnumerable<T> Children<T>(this INode node, string id) where T : class
        {
            var nodes = new List<T>();
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is T n) nodes.Add(n);
            }
        
            return nodes;
        }

        public static IEnumerable<INode> ChildrenInAllLevel(this INode node)
        {
            var children = new List<INode>();
            node.Traverse(children.Add);
            children.Remove(node);
            return children;
        }

        public static IEnumerable<T> ChildrenInAllLevel<T>(this INode node) where T : class
        {
            var children = new List<T>();
        
            node.Traverse(n =>
            {
                if (n is not T child) return;
                children.Add(child);
            });
            
            if (node is T tn && children.Contains(tn))
                children.Remove(tn);
            
            return children;
        }

        public static void AddChild(this INode node, INode childNode)
        {
            if (!childNode.IsRoot())
                childNode.Parent().ChildNode.Remove(childNode);
            
            childNode.ParentNode.Set(node);
            node.ChildNode.Add(childNode);
        }

        public static void AddChildren(this INode node, params INode[] childNodes)
        {
            foreach (var childNode in childNodes)
                node.AddChild(childNode);
        }

        public static void RemoveChild(this INode node, INode childNode)
        {
            if (!node.ChildNode.Contains(childNode)) return;
            
            childNode.ParentNode.SetAsRoot();
            node.ChildNode.Remove(childNode);
        }

        public static void RemoveChildren(this INode node)
        {
            foreach (var childNode in node.ChildNode.Nodes)
                childNode.ParentNode.SetAsRoot();
            
            node.ChildNode.Clear();
        }

        public static INode Neighbour(this INode node) =>
            node.Parent().ChildNode.Nodes.First();
        
        public static T Neighbour<T>(this INode node) where T : class
        {
            foreach (var childNode in node.Parent().ChildNode.Nodes)
                if (childNode is T n) return n;

            throw new Exception($"ChildNode [{typeof(T).Name}] not find");
        }

        public static INode Neighbour(this INode node, string id)
        {
            foreach (var childNode in node.Parent().ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                return childNode;
            }
            
            throw new Exception($"ChildNode [{id}] not find");
        }

        public static T Neighbour<T>(this INode node, string id) where T : class
        {
            foreach (var childNode in node.ParentNode.Node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is T n) return n;
            }
            
            throw new Exception($"ChildNode [{id}] not find");
        }

        public static IEnumerable<INode> Neighbours(this INode node) =>
            node.Parent().ChildNode.Nodes;
        
        public static IEnumerable<T> Neighbours<T>(this INode node) where T : class
        {
            var nodes = new List<T>();
            foreach (var childNode in node.Parent().ChildNode.Nodes)
                if (childNode is T n) nodes.Add(n);
        
            return nodes;
        }

        public static void Traverse(this INode node, Action<INode> action)
        {
            action(node);
            foreach (var n in node.ChildNode.Nodes)
                n.Traverse(action);
        }
    }
}
