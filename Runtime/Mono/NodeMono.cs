using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AceLand.LocalTools.Attribute;
using AceLand.LocalTools.Extensions;
using AceLand.NodeSystem.Base;
using UnityEngine;

namespace AceLand.NodeSystem.Mono
{
    public abstract class NodeMono<T> : MonoBehaviour, INodeMono<T>
        where T : NodeMono<T>
    {
        [Header("Node for Mono")]
        [SerializeField] protected string nodeId;
        [SerializeField] protected bool autoRegister = true;

        [SerializeField] protected MonoBehaviour parentNode;
        [SerializeReference] protected List<MonoBehaviour> childNodes;

        public string Id { get => NodeReady ? _id : nodeId ; private set => _id = value; }
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

        protected bool NodeReady;

        protected virtual void OnValidate()
        {
            if (parentNode == null || parentNode is not INodeMono)
                parentNode = null;

            childNodes ??= new List<MonoBehaviour>();
            List<int> removeList = new();
            for (var i = childNodes.Count - 1; i >= 0; i--)
                if (childNodes[i] is not INodeMono) removeList.Add(i);
            foreach (var i in removeList)
                childNodes.RemoveAt(i);
        }

        protected virtual void Awake()
        {
            Go = gameObject;
            Tr = transform;
            Id = nodeId.IsNullOrEmptyOrWhiteSpace() ? $"{nameof(T)}_{Guid.NewGuid()}" : nodeId;
            ParentNode = new ParentNode(this);
            ChildNode = new ChildNode(this);
            Concrete = (T)this;
            if (autoRegister) Nodes.Register(Concrete);
        }

        protected virtual void Start()
        {
            var pNode = (INodeMono)parentNode;
            var cNodes = childNodes.Cast<INode>().ToArray();
            if (pNode != null) SetParent(pNode);
            if (cNodes is { Length: > 0 }) SetChildren(cNodes);
            StartCoroutine(OnNodeReadyProcess());
        }

        private IEnumerator OnNodeReadyProcess()
        {
            yield return null;
            
            StartAfterNodeBuilt();
        }

        protected virtual void StartAfterNodeBuilt()
        {
            NodeReady = true;
        }

        protected virtual void OnDestroy()
        {
            ParentNode?.Dispose();
            ChildNode?.Dispose();
            Nodes.Unregister(Concrete);
        }

        public virtual void Dispose() => Destroy(this);
        
        
        public void SetId(string id)
        {
            nodeId = id;
            Id = id;
        }

        public void SetParent(INode node)
        {
            parentNode = (MonoBehaviour)node; 
            ParentNode.SetNode(node);
        }

        public void SetChild(INode node)
        {
            childNodes.Add((MonoBehaviour)node);
            ChildNode.Add(node);
        }
        
        public void SetMonoParent<TConcrete>(INodeMono<TConcrete> node)
            where TConcrete : MonoBehaviour
        {
            SetParent(node);
            Tr.SetParent(node.Concrete.transform);
        }
        
        public void SetMonoChild<TConcrete>(INodeMono<TConcrete> node)
            where TConcrete : MonoBehaviour
        {
            SetChild(node);
            node.Concrete.transform.SetParent(Tr);
        }

        public void SetChildren(params INode[] nodes)
        {
            this.childNodes.Clear();
            foreach (var child in nodes)
            {
                this.childNodes.Add((MonoBehaviour)child);
                ChildNode.Add(child);
            }
        }

        public void RemoveChild(INode node)
        {
            childNodes.Remove((MonoBehaviour)node);
        }

        public virtual void Traverse(Action<INode> action)
        {
            action(this);
            foreach (var node in ChildNode.Nodes)
                node.Traverse(action);
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
                foreach (var c in child.GetComponents<INodeMono>())
                {
                    c.ClearNodeStructure();
                }
            }
        }

        private void FindAndSetParentNode()
        {
            if (transform.parent != null && transform.parent.TryGetComponent<INodeMono>(out var nodeMono))
            {
                parentNode = (MonoBehaviour)nodeMono;
            }
            else
            {
                parentNode = null;
            }
        }

        private void FindAndSetChildNode()
        {
            childNodes = new();
            foreach (Transform child in transform)
            {
                foreach (var c in child.GetComponents<INodeMono>())
                {
                    childNodes.Add((MonoBehaviour)c);
                    c.SetNodeStructure();
                }
            }
        }
    }
}
