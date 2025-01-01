using System;
using System.Collections.Generic;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;
using AceLand.TaskUtils;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T> : MonoBehaviour, INode<T>, IMonoNode
        where T : MonoBehaviour, INode
    {
        [Header("Node for Mono")]
        [SerializeField] private string nodeId;
        [SerializeField] private bool autoRegistry = true;

        public string Id { get => NodeReady ? _id : nodeId ; private set => _id = value; }
        private string _id;
        public T Concrete { get; private set; }

        internal ParentNode ParentNode { get; private set; }
        ParentNode INode.ParentNode => ParentNode;

        internal ChildNode ChildNode  { get; private set; }
        ChildNode INode.ChildNode => ChildNode;

        public bool IsActive => gameObject.activeInHierarchy;

        public void SetActive(bool active) => gameObject.SetActive(active);

        public GameObject Go { get; private set; }
        public Transform Tr { get; private set; }
        public bool NodeReady { get; private set; }

        public bool AutoRegistry
        {
            get => autoRegistry;
            set => autoRegistry = value;
        }

        protected virtual void Awake()
        {
            Go = gameObject;
            Tr = transform;
            SetId(nodeId);
            Concrete = GetComponent<T>();
            ParentNode = new ParentNode(Concrete);
            ChildNode = new ChildNode(Concrete);
            if (autoRegistry) Concrete.Register();
            
            var parentNode = FindParentNode();
            var childNodes = FindChildNodes();
            this.LateStart(parentNode, childNodes)
                .Catch(e => Debug.LogError(e, this));
        }

        public virtual void Dispose() => Destroy(this);

        protected virtual void OnDestroy()
        {
            Concrete.Unregister();
            ParentNode?.Dispose();
            ChildNode?.Dispose();
        }

        internal void OnNodeReadyProcess()
        {
            NodeReady = true;
            StartAfterNodeBuilt();
        }

        protected virtual void StartAfterNodeBuilt()
        {
            // override this function to run codes required NodeTree ready
        }

        public void SetId(string id)
        {
            var adjId = id.IsNullOrEmptyOrWhiteSpace() ? $"{nameof(T)}_{Guid.NewGuid()}" : id;
            nodeId = adjId;
            Id = adjId;
        }

        private MonoBehaviour FindParentNode() =>
            transform.parent != null && transform.parent.TryGetComponent<IMonoNode>(out var monoNode)
                ? (MonoBehaviour)monoNode
                : null;

        private List<MonoBehaviour> FindChildNodes()
        {
            var nodes = new List<MonoBehaviour>();
            foreach (Transform child in transform)
            {
                foreach (var monoNode in child.GetComponents<IMonoNode>())
                    nodes.Add((MonoBehaviour)monoNode);
            }

            return nodes;
        }
    }
}
