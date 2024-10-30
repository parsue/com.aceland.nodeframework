using System.Collections.Generic;
using System.Linq;

namespace AceLand.NodeFramework.Base
{
    internal class NodeList
    {
        private readonly List<INode> _nodes = new();

        public INode this[int index] => _nodes[index];  
        public void Add(INode node) => _nodes.Add(node);
        public void Remove(INode node) => _nodes.Remove(node);
        public bool Contains(INode node) => _nodes.Contains(node);
        public void ClearAll() => _nodes.Clear();
        public int Count => _nodes.Count;
        public IEnumerable<INode> List => _nodes;
        public IEnumerable<T> Cast<T>() where T : INode => _nodes.Cast<T>();
        
        public bool TryGetById<T>(string id, out T node) where T : INode
        {
            foreach (var n in _nodes)
            {
                if (n.Id != id) continue;
                node = (T)n;
                return true;
            }

            node =  default;
            return false;
        }
    }
}