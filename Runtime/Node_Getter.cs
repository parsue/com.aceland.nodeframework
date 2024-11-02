using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AceLand.NodeFramework.Core;
using AceLand.TaskUtils;
using UnityEngine;

namespace AceLand.NodeFramework
{
    public partial class Node<T>
    {
        public static Promise<Node<T>> Get() =>
            GetNode();

        public static Promise<Node<T>> Get(string id) =>
            GetNode(id);

        public static IEnumerable<Node<T>> GetNodes() =>
            Nodes.GetNodesByType<Node<T>>();

        private static async Task<Node<T>> GetNode()
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + NodeUtils.Settings.NodeGetterTimeout;
    
            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                var arg = Nodes.TryGetNode(out Node<T> node);
                if (arg) return node;
                
                await Task.Yield();
            }

            var msg = $"Node<{typeof(T).Name}> is not found";
            throw new Exception(msg);
        }

        private static async Task<Node<T>> GetNode(string id)
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + 1.5f;
    
            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                var arg = Nodes.TryGetNode(id, out Node<T> node);
                if (arg) return node;
                
                await Task.Yield();
            }

            var msg = $"Node<{typeof(T).Name}> [{id}] is not found";
            throw new Exception(msg);
        }
    }
}
