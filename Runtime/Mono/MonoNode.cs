using System;
using System.Collections.Generic;
using AceLand.Library.Extensions;
using AceLand.NodeFramework.Core;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public abstract class MonoNode : MonoBehaviour, IMonoNode, INode
    {
        [Header("Node for Mono")]
        [SerializeField] private protected string nodeId;
        [SerializeField] private protected bool autoRegistry = true;

        public string Id { get => NodeReady ? _id : nodeId ; protected set => _id = value; }
        private string _id;
        
        internal ParentNode ParentNode { get; set; }
        ParentNode INode.ParentNode => ParentNode;

        internal ChildNode ChildNode  { get; set; }
        ChildNode INode.ChildNode => ChildNode;


        public virtual bool IsActive => gameObject.activeInHierarchy;

        public virtual void SetActive(bool active) => gameObject.SetActive(active);

        public virtual void Dispose() => Destroy(this);

        public GameObject Go { get; protected set; }
        public Transform Tr { get; protected set; }
        public bool NodeReady { get; protected set; }

        public bool AutoRegistry
        {
            get => autoRegistry;
            set => autoRegistry = value;
        }

        public virtual void SetId(string id)
        {
            var adjId = id.IsNullOrEmptyOrWhiteSpace() ? Guid.NewGuid().ToString() : id;
            nodeId = adjId;
            Id = adjId;
        }

        internal virtual void InitialNode()
        {
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

        internal MonoBehaviour FindParentNode() =>
            transform.parent != null && transform.parent.TryGetComponent<IMonoNode>(out var monoNode)
                ? (MonoBehaviour)monoNode
                : null;

        internal List<MonoBehaviour> FindChildNodes()
        {
            var nodes = new List<MonoBehaviour>();
            foreach (Transform child in transform)
            {
                foreach (var monoNode in child.GetComponents<IMonoNode>())
                    nodes.Add((MonoBehaviour)monoNode);
            }

            return nodes;
        }

        public int CompareTo(object obj)
        {
            if (obj is not INode other) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (GetType() != obj.GetType()) return 1;

            return CompareTo(other);
        }

        public int CompareTo(INode other)
        {
            if (other == null) return 1;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (Id.IsNullOrEmptyOrWhiteSpace()) return 1;
            if (GetType() != other.GetType()) return 1;
            
            return string.CompareOrdinal(Id, other.Id); 
        }

        public bool Equals(INode other)
        {
            if (other == null) return false;
            if (other.Id.IsNullOrEmptyOrWhiteSpace()) return false;
            if (Id.IsNullOrEmptyOrWhiteSpace()) return false;
            
            return Id == other.Id && GetType() == other.GetType();
        }
    }
}
