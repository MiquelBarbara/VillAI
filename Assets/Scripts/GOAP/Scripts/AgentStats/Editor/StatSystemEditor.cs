using UnityEditor;
using GOAP.Scripts;
using System.Collections.Generic;
using GOAP.Scripts.AgentStats;

[CustomEditor(typeof(StatSystem))]
public class StatSystemEditor : Editor
{
    private StatSystem statSystem;

    private void OnEnable()
    {
        statSystem = (StatSystem)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Stats Overview", EditorStyles.boldLabel);

        List<Stat> stats = statSystem.GetAllStats();

        if (stats == null || stats.Count == 0)
        {
            EditorGUILayout.HelpBox("No stats available.", MessageType.Warning);
        }
        else
        {
            foreach (var stat in stats)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(stat.Key, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Value: ", stat.Value.ToString("F2"));
                EditorGUILayout.EndVertical();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}