using AceLand.NodeFramework.ProjectSetting;
using UnityEngine;

namespace AceLand.NodeFramework.Core
{
    internal static class NodeUtils
    {
        public static NodeFrameworkSettings Settings
        {
            get
            {
                _settings ??= Resources.Load<NodeFrameworkSettings>(nameof(NodeFrameworkSettings));
                return _settings;
            }
        }
        
        private static NodeFrameworkSettings _settings;
    }
}