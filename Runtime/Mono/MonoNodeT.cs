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

        protected virtual void Awake()
        {
            this.InitialMonoNode();
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

        public override void SetId(string id)
        {
            var adjId = id.IsNullOrEmptyOrWhiteSpace() ? $"{nameof(T)}_{Guid.NewGuid()}" : id;
            nodeId = adjId;
            Id = adjId;
        }
    }
}
