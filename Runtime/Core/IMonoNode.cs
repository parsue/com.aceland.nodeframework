using UnityEngine;

namespace AceLand.NodeFramework.Core
{
    internal interface IMonoNode
    {
        void SetActive(bool active);
        bool IsActive { get; }
        bool NodeReady { get; }
        bool AutoRegistry { get; set; }
        Transform Tr { get; }
        GameObject Go { get; }
        void SetId(string id);
        void SetNodeStructure();
        void ClearNodeStructure();
    }
}