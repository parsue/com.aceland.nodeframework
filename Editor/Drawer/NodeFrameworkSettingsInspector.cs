using AceLand.Library.Editor;
using AceLand.NodeFramework.ProjectSetting;
using UnityEditor;

namespace AceLand.NodeFramework.Editor.Drawer
{
    [CustomEditor(typeof(NodeFrameworkSettings))]
    public class NodeFrameworkSettingsInspector : UnityEditor.Editor
    {   
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorHelper.DrawAllPropertiesAsDisabled(serializedObject);
        }
    }
}