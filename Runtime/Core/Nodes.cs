using System;
using System.Collections.Generic;

namespace AceLand.NodeFramework.Core
{
    internal static class Nodes
    {
        private static readonly Dictionary<Type, NodeList> NODES_BY_TYPE = new();

        public static void Register<T>(T node) where T : INode
        {
            var type = typeof(T);
            if (!NODES_BY_TYPE.ContainsKey(type)) NODES_BY_TYPE[type] = new NodeList();
            if (NODES_BY_TYPE[type].Contains(node))
                throw new Exception($"Nodes.Register Error: node {typeof(T).Name} [{node.Id}] already exists");
                
            NODES_BY_TYPE[type].Add(node);
        }

        public static void Unregister<T>(T node) where T : INode
        {
            var type = typeof(T);
            if (!NODES_BY_TYPE.TryGetValue(type, out var list)) return;
            if (!list.Contains(node)) return;
            
            NODES_BY_TYPE[type].Remove(node);
        }

        public static bool TryGetNode<T>(out T node) where T : INode
        {
            var type = typeof(T);
            if (NODES_BY_TYPE.TryGetValue(type, out var nodes))
            {
                node = (T)nodes[0];
                return true;
            }

            node = default;
            return false;
        }

        public static bool TryGetNode<T>(string id, out T node) where T : INode
        {
            var type = typeof(T);
            var checkType = NODES_BY_TYPE.TryGetValue(type, out var nodes);

            if (checkType) 
                return nodes.TryGetById(id, out node);
            
            node = default;
            return false;
        }
        
        public static IEnumerable<T> GetNodesByType<T>() where T : INode
        {
            var type = typeof(T);
            if (!NODES_BY_TYPE.TryGetValue(type, out var nodes)) yield break;

            foreach (var node in nodes.List)
                yield return (T)node;
        }

        public static bool Contains<T>(INode node) where T : INode
        {
            var type = typeof(T);
            return NODES_BY_TYPE.TryGetValue(type, out var nodes) && nodes.Contains(node);
        }
    }
}
