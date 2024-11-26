using AceLand.Library.Editor.Providers;
using AceLand.NodeFramework.ProjectSetting;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.NodeFramework.Editor.ProjectSettingsProvider
{
    public class NodeFrameworkSettingsProvider : AceLandSettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand Node Framework";
        
        private NodeFrameworkSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        [InitializeOnLoadMethod]
        public static void CreateSettings() => NodeFrameworkSettings.GetSerializedSettings();
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            Settings = NodeFrameworkSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new NodeFrameworkSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }
    }
}