using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AceLand.Library.Attribute;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Base;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public abstract class MonoNode<T> : MonoBehaviour, INode<T>, IMonoNode
        where T : MonoBehaviour
    {
        [Header("Node for Mono")]
        [SerializeField] private string nodeId;
        [SerializeField] internal protected MonoBehaviour parentNode;
        [SerializeReference] internal protected List<MonoBehaviour> childNodes;

        public string Id { get => NodeReady ? _id : nodeId ; private set => _id = value; }
        private string _id;
        public T Concrete { get; private set; }

        internal protected ParentNode ParentNode { get; private set; }
        ParentNode INode.ParentNode => ParentNode;

        internal protected ChildNode ChildNode  { get; private set; }
        ChildNode INode.ChildNode => ChildNode;

        public bool IsActive => gameObject.activeInHierarchy;

        public void SetActive(bool active) => gameObject.SetActive(active);

        public GameObject Go { get; private set; }
        public Transform Tr { get; private set; }
        public bool NodeReady { get; private set; }

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
        }

        protected virtual void Start()
        {
            SetNode();
            StartCoroutine(OnNodeReadyProcess());
        }

        public virtual void Dispose() => Destroy(this);

        protected virtual void OnDestroy()
        {
            ParentNode?.Dispose();
            ChildNode?.Dispose();
            Nodes.Unregister(this);
        }

        private void SetNode()
        {
            var pNode = (INode)parentNode;
            var cNodes = childNodes.Cast<INode>().ToArray();
            
            SetId(nodeId);
            Concrete = GetComponent<T>();
            ParentNode = pNode == null
                ? new ParentNode(this)
                : new ParentNode(this, pNode);
            ChildNode = cNodes is { Length: > 0 }
                ? new ChildNode(this, cNodes)
                : new ChildNode(this);
            Nodes.Register(this);
            
            NodeReady = true;
        }

        private IEnumerator OnNodeReadyProcess()
        {
            yield return null;
            
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
