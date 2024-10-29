using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AceLand.Library.Attribute;
using AceLand.Library.Extensions;
using AceLand.NodeSystem.Base;
using UnityEngine;

namespace AceLand.NodeSystem.Mono
{
    public abstract class MonoNode<T> : MonoBehaviour, INode<T>, IMonoNode
        where T : MonoBehaviour
    {
        [Header("Node for Mono")]
        [SerializeField] protected string nodeId;
        [SerializeField] protected MonoBehaviour parentNode;
        [SerializeReference] protected List<MonoBehaviour> childNodes;

        public string Id { get => _nodeReady ? _id : nodeId ; private set => _id = value; }
        private string _id;
        public T Concrete { get; private set; }

        private ParentNode ParentNode { get; set; }
        ParentNode INode.ParentNode => ParentNode;

        private ChildNode ChildNode  { get; set; }
        ChildNode INode.ChildNode => ChildNode;

        public bool IsActive => gameObject.activeInHierarchy;

        public void SetActive(bool active) => gameObject.SetActive(active);

        public GameObject Go { get; private set; }
        public Transform Tr { get; private set; }

        bool IMonoNode.NodeReady => _nodeReady;
        private bool _nodeReady;

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
            ParentNode = new ParentNode(this);
            ChildNode = new ChildNode(this);
            Concrete = GetComponent<T>();
            Nodes.Register(this);
        }

        protected virtual void Start()
        {
            var pNode = (INode)parentNode;
            var cNodes = childNodes.Cast<INode>().ToArray();
            if (pNode != null) this.SetParent(pNode);
            if (cNodes is { Length: > 0 }) this.AddChildren(cNodes);
            StartCoroutine(OnNodeReadyProcess());
        }

        private IEnumerator OnNodeReadyProcess()
        {
            yield return null;
            
            StartAfterNodeBuilt();
        }

        protected virtual void StartAfterNodeBuilt()
        {
            _nodeReady = true;
        }

        protected virtual void OnDestroy()
        {
            ParentNode?.Dispose();
            ChildNode?.Dispose();
            Nodes.Unregister(this);
        }

        public virtual void Dispose() => Destroy(this);
        
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
        }

        private void FindAndSetParentNode()
        {
            if (transform.parent != null && transform.parent.TryGetComponent<IMonoNode>(out var nodeMono))
                parentNode = (MonoBehaviour)nodeMono;
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
