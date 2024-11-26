using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AceLand.NodeFramework.Core;
using AceLand.TaskUtils;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T>
    {
        public static Task<T> GetAsync() =>
            GetNode();

        public static Task<T> GetAsync(string id) =>
            GetNode(id);

        public static Task<T> GetAsync<TEnum>(TEnum id) where TEnum : Enum =>
            GetNode(id.ToString());

        public static T Get() =>
            Nodes.TryGetNode(out T node) ? node : null;

        public static T Get(string id) =>
            Nodes.TryGetNode(id, out T node) ? node : null;

        public static T Get<TEnum>(TEnum id) where TEnum : Enum =>
            Get(id.ToString());

        public static IEnumerable<T> GetNodes() =>
            Nodes.GetNodesByType<T>();

        private static async Task<T> GetNode()
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

        private static async Task<T> GetNode(string id)
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
