﻿using System;
using System.Collections.Generic;
using System.Linq;
using AceLand.NodeFramework.Core;
using UnityEngine;

namespace AceLand.NodeFramework.Mono
{
    public static class MonoNodeExtension
    {
        public static T MonoParent<T>(this INode node) where T : MonoBehaviour, INode
        {
            if (node.ParentNode.Node is T parent)
                return parent;
            
            throw new Exception($"wrong type of {nameof(T)}");
        }
        
        public static T MonoChild<T>(this INode node) where T : MonoBehaviour, INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
                if (childNode is T n) return n;
            
            throw new Exception($"ChildNode [{nameof(T)}] not find");
        }
        
        public static T MonoChild<T>(this INode node, string id) where T : MonoBehaviour, INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is T n) return n;
            }
            
            throw new Exception($"ChildNode [{id}] of [{nameof(T)}] not find");
        }
        
        public static IEnumerable<T> MonoChildren<T>(this INode node) where T : MonoBehaviour, INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode is T n) yield return n;
            }
        }

        public static IEnumerable<T> MonoChildren<T>(this INode node, string id) where T : MonoBehaviour, INode
        {
            var nodes = new List<T>();
            foreach (var childNode in node.ChildNode.Nodes)
            {
                if (childNode.Id != id) continue;
                if (childNode is T n) nodes.Add(n);
            }
        
            return nodes;
        }

        public static IEnumerable<T> MonoChildrenInAllLevel<T>(this INode node) where T : MonoBehaviour, INode
        {
            var children = new List<T>();
        
            node.Traverse(n =>
            {
                if (n is not T child) return;
                children.Add(child);
            });
            
            if (node is T tn && children.Contains(node))
                children.Remove(tn);
            
            return children;
        }
        
        public static void SetParent<T>(this MonoNode<T> node, INode parentNode) where T : MonoBehaviour, INode
        {
            if (parentNode is not MonoBehaviour mono) return;
            
            if (!node.IsRoot())
                node.Parent().ChildNode.Remove(node);
            
            parentNode.ChildNode?.Add(node);
            node.ParentNode?.Set(parentNode);
            node.transform.SetParent(mono.transform);
        }

        public static void SetAsRoot<T>(this MonoNode<T> node) where T : MonoBehaviour, INode
        {
            if (!node.IsRoot())
                node.Parent().ChildNode.Remove(node);
            
            node.ParentNode.SetAsRoot();
            node.transform.SetParent(null);
        }

        public static void AddChild<T>(this MonoNode<T> node, INode childNode) where T : MonoBehaviour, INode
        {
            if (childNode is not MonoBehaviour mono) return;
            
            if (!childNode.IsRoot())
                childNode.Parent().ChildNode.Remove(childNode);
            
            childNode.ParentNode.Set(node);
            node.ChildNode.Add(childNode);
            mono.transform.SetParent(node.transform);
        }

        public static void AddChildren<T>(this MonoNode<T> node, params INode[] childNodes) where T : MonoBehaviour, INode
        {
            if (childNodes.Any(n => n is not MonoBehaviour)) return;
            
            foreach (var childNode in childNodes)
            {
                var mono = childNode as MonoBehaviour;
                node.AddChild(childNode);
                mono.transform.SetParent(node.Tr);
            }
        }

        public static void RemoveChild<T>(this MonoNode<T> node, INode childNode) where T : MonoBehaviour, INode
        {
            if (childNode is not MonoBehaviour mono) return;
            if (!node.ChildNode.Contains(childNode)) return;
            
            childNode.ParentNode.SetAsRoot();
            node.ChildNode.Remove(childNode);
            mono.transform.SetParent(null);
        }

        public static void RemoveChildren<T>(this MonoNode<T> node) where T : MonoBehaviour, INode
        {
            foreach (var childNode in node.ChildNode.Nodes)
            {
                var mono = childNode as MonoBehaviour;
                childNode.ParentNode.SetAsRoot();
                mono.transform.SetParent(null);
            }
            
            node.ChildNode.Clear();
        }
    }
}