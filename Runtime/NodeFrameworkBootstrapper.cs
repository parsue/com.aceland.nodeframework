using AceLand.NodeFramework.ProjectSetting;
using UnityEngine;

namespace AceLand.NodeFramework
{
    internal static class NodeFrameworkBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialization()
        {
            Node.Settings = Resources.Load<NodeFrameworkSettings>(nameof(NodeFrameworkSettings));
            
        }
    }
}