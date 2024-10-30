using UnityEngine;

namespace AceLand.NodeFramework.Base
{
    public interface IMonoNode
    {
        void SetActive(bool active);
        bool IsActive { get; }
        internal bool NodeReady { get; }
        Transform Tr { get; }
        GameObject Go { get; }
        void SetId(string id);
        void SetNodeStructure();
        void ClearNodeStructure();
    }
}