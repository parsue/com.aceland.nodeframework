using AceLand.Library.Editor;
using AceLand.NodeFramework.ProjectSetting;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.NodeFramework.Editor.ProjectSettingsProvider
{
    public class NodeFrameworkSettingsProvider : SettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand Node Framework";
        private SerializedObject _settings;
        
        private NodeFrameworkSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        [InitializeOnLoadMethod]
        public static void CreateSettings() => NodeFrameworkSettings.GetSerializedSettings();
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = NodeFrameworkSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new NodeFrameworkSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }

        public override void OnGUI(string searchContext)
        {
            EditorHelper.DrawAllProperties(_settings);
        }
    }
}