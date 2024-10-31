using AceLand.Library.ProjectSetting;
using UnityEngine;

namespace AceLand.NodeFramework.ProjectSetting
{
    public class NodeFrameworkSettings : ProjectSettings<NodeFrameworkSettings>
    {
        [Header("Signal")] 
        [SerializeField] private float nodeGetterTimeout = 1.5f;

        public float NodeGetterTimeout => nodeGetterTimeout;
    }
}