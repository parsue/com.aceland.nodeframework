using System;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T> : MonoNode, INode<T>
        where T : MonoBehaviour, INode
    {
        public T Concrete { get; private set; }

        private event Action<T> _onNodeTReady;

        protected virtual void Awake()
        {
            this.InitialMonoNode(setAsRoot);
        }

        protected virtual void OnDestroy()
        {
            Concrete.Unregister();
            ParentNode?.Dispose();
            ChildNode?.Dispose();
        }

        internal override void InitialNode()
        {
            Go = gameObject;
            Tr = transform;
            SetId(nodeId);
            Concrete = GetComponent<T>();
            ParentNode = new ParentNode(Concrete);
            ChildNode = new ChildNode(Concrete);
            if (autoRegistry) Concrete.Register();
        }

        public void OnNodeReadyAction(Action<T> action)
        {
            if (NodeReady)
            {
                action?.Invoke(Concrete);
                return;
            }

            _onNodeTReady += action;
        }

        internal override void OnNodeReadyProcess()
        {
            base.OnNodeReadyProcess();
            _onNodeTReady?.Invoke(Concrete);
        }

        public override void SetId(string id)
        {
            var adjId = id.IsNullOrEmptyOrWhiteSpace() ? $"{typeof(T).Name}_{Guid.NewGuid()}" : id;
            nodeId = adjId;
            Id = adjId;
        }
    }
}
