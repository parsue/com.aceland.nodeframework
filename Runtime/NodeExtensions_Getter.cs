using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AceLand.NodeFramework.Core;
using AceLand.TaskUtils;

namespace AceLand.NodeFramework
{
    public static partial class NodeExtensions
    {
        public static Task<T> GetAsync<T>(this INode<T> node) where T : class, INode =>
            GetNode<T>();
        
        public static Task<T> GetAsync<T>(this INode<T> node, string id) where T : class, INode =>
            GetNode<T>(id);
        
        public static Task<T> GetAsync<T, TEnum>(this INode<T> node, TEnum id) 
            where T : class, INode 
            where TEnum : Enum =>
            GetNode<T>(id.ToString());

        public static T Get<T>(this INode<T> node) where T : class, INode =>
            Nodes.TryGetNode(out T n) ? n : null;
        
        public static T Get<T>(this INode<T> node, string id) where T : class, INode =>
            Nodes.TryGetNode(id, out T n) ? n : null;
        
        public static T Get<T, TEnum>(this INode<T> node, TEnum id)
            where T : class, INode 
            where TEnum : Enum =>
            Get(node, id.ToString());
        
        public static IEnumerable<T> GetNodes<T>(this INode<T> node) where T : class, INode =>
            Nodes.GetNodesByType<T>();
        
        
        // local functions
        private static async Task<T> GetNode<T>() where T : class, INode
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = DateTime.Now.AddSeconds(NodeUtils.Settings.NodeGetterTimeout);
    
            while (!aliveToken.IsCancellationRequested && DateTime.Now < targetTime)
            {
                var arg = Nodes.TryGetNode(out T node);
                if (arg) return node;
                
                await Task.Yield();
            }

            var msg = $"Node<{typeof(T).Name}> is not found";
            throw new Exception(msg);
        }

        private static async Task<T> GetNode<T>(string id) where T : class, INode
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = DateTime.Now.AddSeconds(NodeUtils.Settings.NodeGetterTimeout);
    
            while (!aliveToken.IsCancellationRequested && DateTime.Now < targetTime)
            {
                var arg = Nodes.TryGetNode(id, out T node);
                if (arg) return node;
                
                await Task.Yield();
            }

            var msg = $"Node<{typeof(T).Name}> [{id}] is not found";
            throw new Exception(msg);
        }
    }
}
