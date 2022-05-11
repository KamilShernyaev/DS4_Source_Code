using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UltimateSpawner.EditorScript
{
    [CustomEditor(typeof(InfiniteController))]
    public class InfiniteControllerInspector : Editor
    {
        // Methods
        public override void OnInspectorGUI()
        {
            // Get the spawner
            InfiniteController spawner = target as InfiniteController;

            // Draw common properties
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnManager"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumSpawnCount"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumSpawnCount"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnDelay"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stopAfterTime"), true);

            // Check for expanded
            if (spawner.stopAfterTime == true)
            {
                // Draw the time property
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopAfter"), true);
            }

            // Update the object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
