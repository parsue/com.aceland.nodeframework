using System.Linq;
using AceLand.NodeFramework.Core;
using UnityEditor;
using UnityEngine;

namespace AceLand.NodeFramework.Editor.MenuItem
{
    public static class NodeMenuItem
    {
        [UnityEditor.MenuItem("GameObject/Node/Enable Auto Register", priority = 0)]
        public static void EnableAutoRegister()
        {
            SetAutoRegister(true);
        }
        
        [UnityEditor.MenuItem("GameObject/Node/Disable Auto Register", priority = 1)]
        public static void DisableAutoRegister()
        {
            SetAutoRegister(false);
        }

        private static void SetAutoRegister(bool value)
        {
            var selectedObject = Selection.activeGameObject;
            if (selectedObject == null) return;
            
            var childObjects = selectedObject.GetComponentsInChildren<Transform>()
                .Select(t => t.gameObject)
                .ToArray();

            foreach (var childObject in childObjects)
            {
                if (!childObject.TryGetComponent<IMonoNode>(out var monoNode)) continue;
                monoNode.AutoRegistry = value;
            }
            
            EditorUtility.SetDirty(selectedObject);
        }
    }
}