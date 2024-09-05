﻿using System;
using UnityEngine;

namespace AceLand.NodeSystem.Base
{
    public interface INodeMono<T> : INodeMono
        where T : MonoBehaviour
    {
        T Concrete { get; }
        void Traverse(Action<T> action);

        void SetMonoParent<TConcrete>(INodeMono<TConcrete> node)
            where TConcrete : MonoBehaviour;
        void SetMonoChild<TConcrete>(INodeMono<TConcrete> node)
            where TConcrete : MonoBehaviour;
    }

    public interface INodeMono : INode
    {
        void SetNodeStructure();
        void ClearNodeStructure();
    }
}