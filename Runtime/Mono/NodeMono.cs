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
    public abstract class NodeMono<T> : MonoBehaviour, INodeMono<T>
        where T : NodeMono<T>
    {
        [Header("Node for Mono")]
        [SerializeField] protected string nodeId;
        [SerializeField] protected bool autoRegister = true;

        [SerializeField] protected MonoBehaviour parentNode;
        [SerializeReference] protected List<MonoBehaviour> childNodes;

        public string Id { get; private set; }
        public T Concrete { get; private set; }

        public ParentNode ParentNode { get; private set; }
        public ChildNode ChildNode  { get; private set; }

        public bool IsActive => gameObject.activeInHierarchy;
        public void SetActive(bool active) => gameObject.SetActive(active);

        public GameObject Go { get; private set; }
        public Transform Tr { get; private set; }

        protected bool NodeReady;

        public virtual void OnValidate()
        {
            if (parentNode == null || parentNode is not INodeMono)
                parentNode = null;

            childNodes ??= new();
            List<int> removeList = new();
            for (var i = childNodes.Count - 1; i >= 0; i--)
                if (childNodes[i] is not INodeMono) removeList.Add(i);
            foreach (var i in removeList)
                childNodes.RemoveAt(i);
        }

        public virtual void Awake()
        {
            Go = gameObject;
            Tr = transform;
            Id = nodeId.IsNullOrEmptyOrWhiteSpace() ? $"{nameof(T)}_{Guid.NewGuid()}" : nodeId;
            ParentNode = new(this);
            ChildNode = new(this);
            Concrete = (T)this;
            if (autoRegister) Nodes.Register(Concrete);
        }

        public virtual void Start()
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

        public virtual void OnDestroy()
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

        public void SetParent(INode parentNode)
        {
            this.parentNode = (MonoBehaviour)parentNode; 
            ParentNode.SetParent(parentNode);
        }

        public void SetChild(INode childNode)
        {
            childNodes = new() { (MonoBehaviour)childNode };
            ChildNode.Add(childNode);
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

        public void SetChildren(params INode[] childNodes)
        {
            this.childNodes.Clear();
            foreach (var child in childNodes)
            {
                this.childNodes.Add((MonoBehaviour)child);
                ChildNode.Add(child);
            }
        }

        public void RemoveChild(INode childNode)
        {
            childNodes.Remove((MonoBehaviour)childNode);
        }

        public virtual void Traverse(Action<T> action)
        {
            action(Concrete);
            foreach (var node in ChildNode.Nodes)
            {
                var tNode = (T)node;
                tNode.Traverse(action);
            }
        }

        public virtual void Traverse(Action<INode> action)
        {
            action(this);
            foreach (var node in ChildNode.Nodes)
                node.Traverse(action);
        }

#if UNITY_EDITOR

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

#endif
    }
}