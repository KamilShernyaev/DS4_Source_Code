using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UltimateSpawner.EditorScript
{
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPointInspector : Editor
    {
        // Private

        // Public

        // Properties

        // Methods
        public override void OnInspectorGUI()
        {
            // Get the target
            SpawnPoint point = target as SpawnPoint;

            // Draw all categories
            drawSpawner(point);
            drawPoint(point);
            drawEditor(point);

            // Apply changes to the object
            serializedObject.ApplyModifiedProperties();


            // Check for trigger mode
            if (point.occupiedCheckMode == SpawnPoint.OccupiedCheckMode.TriggerEnterExit)
            {
                // Check for a collider that is a trigger
                bool error = true;
                Collider[] colliders = point.GetComponents<Collider>();

                // Check if any are triggers
                foreach (Collider collider in colliders)
                {
                    // The spawn is valid
                    if (collider.isTrigger == true)
                        error = false;
                }

                // Check for a collider 2D that is a trigger
                Collider2D[] colliders2D = point.GetComponents<Collider2D>();

                // Check if any are triggers
                foreach(Collider2D collider in colliders2D)
                {
                    // The spawn is valid
                    if (collider.isTrigger == true)
                        error = false;
                }

                // Not valid
                if (error == true)
                    EditorGUILayout.HelpBox("The occupied detect mode is set to 'Triggers' but the spawn point does not contain a trigger collider. Make sure to add a Trigger collider or you may experience overlapped objects. Alternativley you can switch to sphere overlap mode.", MessageType.Error);
            }

            EditorGUILayout.Space();

            // Check if the spawn point is free
            bool available = point.IsSpawnFree;

            // Form the message string
            string message = (available == true) ? "Available" : "Not Available";

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Spawn Status: ", EditorStyles.boldLabel, GUILayout.Width(100));
                EditorGUILayout.LabelField(message, spawnStatusStyle(available));
            }
            GUILayout.EndHorizontal();
        }

        private void drawSpawner(SpawnPoint point)
        {
            // Spawner
            EditorGUILayout.LabelField("Spawner", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnSettings"), true);

            // Check for custom mode
            if (point.SpawnSettings == SpawnSettings.Custom)
            {
                // Draw a slot for custom spawners
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spawner"), true);
            }

            // Space
            EditorGUILayout.Space();
        }

        private void drawPoint(SpawnPoint point)
        {
            // Spawn point
            EditorGUILayout.LabelField("Spawn Point", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("performOccupiedCheck"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnRadius"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("occupiedCheckMode"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("collisionLayer"), true);

            // Space
            EditorGUILayout.Space();
        }

        private void drawEditor(SpawnPoint point)
        {
            // Editor
            EditorGUILayout.LabelField("Editor Specific", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colliderColour"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("directionColour"), true);
        }

        private GUIStyle spawnStatusStyle(bool available)
        {
            // Copy the style
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);

            // Change the colour
            style.normal.textColor = (available == true) ? Color.green : Color.red;

            return style;
        }
    }
}