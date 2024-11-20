using AceLand.NodeFramework.ProjectSetting;
using UnityEditor;

namespace AceLand.NodeFramework.Editor.ProjectSettingsProvider
{
    [InitializeOnLoad]
    public static class PackageInitializer
    {
        static PackageInitializer()
        {
            NodeFrameworkSettings.GetSerializedSettings();
        }
    }
}