using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AceLand.Library.Attribute;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public abstract partial class MonoNode<T> : MonoBehaviour, INode<T>, IMonoNode
        where T : MonoBehaviour, INode
    {
        [Header("Node for Mono")]
        [SerializeField] private string nodeId;
        [SerializeField] protected internal MonoBehaviour parentNode;
        [SerializeReference] protected internal List<MonoBehaviour> childNodes;
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

        protected virtual void OnValidate()
        {
            if (parentNode == null || parentNode is not INode)
                parentNode = null;

            childNodes ??= new List<MonoBehaviour>();
            List<int> removeList = new();
            for (var i = childNodes.Count - 1; i >= 0; i--)
                if (childNodes[i] is not INode) removeList.Add(i);
            foreach (var i in removeList)
                childNodes.RemoveAt(i);
        }

        protected virtual void Awake()
        {
            Go = gameObject;
            Tr = transform;
            SetId(nodeId);
            Concrete = GetComponent<T>();
            if (autoRegistry) Register();
        }

        protected virtual void Start()
        {
            SetNode();
            StartCoroutine(OnNodeReadyProcess());
        }

        public virtual void Dispose() => Destroy(this);

        protected virtual void OnDestroy()
        {
            Unregister();
            ParentNode?.Dispose();
            ChildNode?.Dispose();
        }
        
        protected void Register() => Nodes.Register(Concrete);
        protected void Unregister() => Nodes.Unregister(Concrete);

        private void SetNode()
        {
            var pNode = (INode)parentNode;
            var cNodes = childNodes.Cast<INode>().ToArray();
            
            ParentNode = pNode == null
                ? new ParentNode(this)
                : new ParentNode(this, pNode);
            ChildNode = cNodes is { Length: > 0 }
                ? new ChildNode(this, cNodes)
                : new ChildNode(this);
        }

        private IEnumerator OnNodeReadyProcess()
        {
            yield return null;
            StartAfterNodeBuilt();
            NodeReady = true;
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

        [InspectorButton]
        public void SetNodeStructure()
        {
            FindAndSetParentNode();
            FindAndSetChildNode();
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            
        }

        [InspectorButton]
        public void ClearNodeStructure()
        {
            parentNode = null;
            childNodes.Clear();
            foreach (Transform child in transform)
            {
                foreach (var monoNode in child.GetComponents<IMonoNode>())
                {
                    monoNode.ClearNodeStructure();
                }
            }
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

        }

        private void FindAndSetParentNode()
        {
            if (transform.parent != null && transform.parent.TryGetComponent<IMonoNode>(out var monoNode))
                parentNode = (MonoBehaviour)monoNode;
            else
                parentNode = null;
        }

        private void FindAndSetChildNode()
        {
            childNodes = new List<MonoBehaviour>();
            foreach (Transform child in transform)
            {
                foreach (var monoNode in child.GetComponents<IMonoNode>())
                {
                    childNodes.Add((MonoBehaviour)monoNode);
                    monoNode.SetNodeStructure();
                }
            }
        }
    }
}
