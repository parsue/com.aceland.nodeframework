using UnityEngine;

namespace AceLand.NodeFramework.Core
{
    internal interface IMonoNode
    {
        void SetActive(bool active);
        bool IsActive { get; }
        bool NodeReady { get; }
        Transform Tr { get; }
        GameObject Go { get; }
        void SetId(string id);
        void SetNodeStructure();
        void ClearNodeStructure();
    }
}