using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UltimateSpawner.EditorScript
{
    [CustomEditor(typeof(SpawnArea))]
    public class SpawnAreaInspector : Editor
    {
        // Private

        // Public

        // Properties

        // Methods
        public override void OnInspectorGUI()
        {
            // Get the spawn area
            SpawnArea area = target as SpawnArea;

            drawSpawner(area);
            drawArea(area);

            // Save changes
            serializedObject.ApplyModifiedProperties();

            // Make sure the area is active
            if (area.alwaysActive == false)
            {
                // Check for trigger mode
                Collider[] colliders = area.GetComponents<Collider>();

                // Check if any are triggers
                foreach (Collider collider in colliders)
                {
                    // The spawn is valid
                    if (collider.isTrigger == true)
                        return;
                }

                // Check for 2 trigger mode
                Collider2D[] colliders2D = area.GetComponents<Collider2D>();

                // Check if any are triggers
                foreach(Collider2D collider in colliders2D)
                {
                    // The spawn is valid
                    if (collider.isTrigger == true)
                        return;
                }

                // Not valid
                EditorGUILayout.HelpBox("The spawn area does not contain a trigger collider. Make sure to add a Trigger collider or you area will not work as expected.", MessageType.Error);
            }
        }

        private void drawSpawner(SpawnArea area)
        {
            EditorGUILayout.LabelField("Spawner", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnSettings"), true);

            // Check for custom
            if (area.spawnSettings == SpawnSettings.Custom)
            {
                // Draw slot for custom spawner
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spawner"), true);
            }
        }

        private void drawArea(SpawnArea area)
        {
            EditorGUILayout.LabelField("Spawn Area", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnMode"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("alwaysActive"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("areaTag"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("collisionLayer"), true);
        }
    }
}