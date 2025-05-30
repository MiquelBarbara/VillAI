using UnityEditor;
using UnityEngine;
using Utilities.ScriptableObjectExtensions.Editor;

namespace Systems.SaveSystem.Memory.Conversations.Editor
{
    [CustomEditor(typeof(ConversationContainer), true)] // 'true' enables inheritance
    public class ConversationContainerEditor : ScriptableSaveEditor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector (from ScriptableSave's editor)
            base.OnInspectorGUI();

            ConversationContainer container = (ConversationContainer)target;

            // Add custom buttons
            GUILayout.Space(10);
            if (GUILayout.Button("Add New Group"))
            {
                //Open a windows and ask for a name
                container.CreateGroup(name.ToString());
            }
            
            if (GUILayout.Button("Remove Group"))
            {
                container.RemoveGroup("New_Character");
            }
        }
    }
}