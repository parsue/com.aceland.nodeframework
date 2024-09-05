using System;
using System.Collections.Generic;
using AceLand.NodeSystem.Base;

namespace AceLand.NodeSystem
{
    public static class Nodes
    {
        private static readonly Dictionary<Type, NodeList> _nodes = new();

        public static void Register<T>(T node) where T : INode
        {
            var type = typeof(T);
            if (!_nodes.ContainsKey(type)) _nodes[type] = new();
            if (_nodes[type].Contains(node))
                throw new Exception($"Nodes.Register Error: node [{node.Id}] already exists");
                
            _nodes[type].Add(node);
        }

        public static void Unregister<T>(T node) where T : INode
        {
            var type = typeof(T);
            if (!_nodes.TryGetValue(type, out var list)) return;
            if (!list.Contains(node)) return;
            
            _nodes[type].Remove(node);
        }

        public static T Get<T>() where T : INode
        {
            var type = typeof(T);
            return !_nodes.TryGetValue(type, out var nodes) 
                ? default 
                : (T)nodes[0];
        }

        public static T Get<T>(string id) where T : INode
        {
            var type = typeof(T);
            return !_nodes.TryGetValue(type, out var nodes) 
                ? default 
                : nodes.GetById<T>(id);
        }
        
        public static IEnumerable<T> GetNodesByType<T>() where T : INode
        {
            var type = typeof(T);
            if (!_nodes.TryGetValue(type, out var nodes)) yield break;

            foreach (var node in nodes.List)
                yield return (T)node;
        }

        public static bool Contains<T>(INode node) where T : INode
        {
            var type = typeof(T);
            return _nodes.TryGetValue(type, out var nodes) && nodes.Contains(node);
        }
    }
}