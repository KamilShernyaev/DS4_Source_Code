using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UltimateSpawner.EditorScript
{
    [CustomEditor(typeof(WaveController))]
    public class WaveControllerInspector : Editor
    {
        // Methods
        public override void OnInspectorGUI()
        {
            // Get the manager
            WaveController manager = target as WaveController;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnManager"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumSpawnedItems"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onComplete"), true);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("waveMode"), true);
            {
                // Show the continuos options for the controller
                if(manager.waveMode == WaveController.WaveControllerMode.Continuous || manager.waveMode == WaveController.WaveControllerMode.Mixed)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnAdvanceRate"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("delayAdvanceRate"), true);

                    // Hide the start wave for mixed mode - it carrys on from the preset wave
                    if(manager.waveMode == WaveController.WaveControllerMode.Continuous)
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("startWave"), true);
                }

                // Show the preset options for the controller
                if(manager.waveMode == WaveController.WaveControllerMode.Preset || manager.waveMode == WaveController.WaveControllerMode.Mixed)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("userWaves"), true);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("useBossWaves"), true);
            {
                if (manager.useBossWaves == true)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bossRoundIsWave"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bossWaveRound"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bossWave"), true);
                }
            }

            // Update changes
            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying == true)
            {
                // Display spawner stats
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    // Disable GUI
                    bool old = GUI.enabled;
                    GUI.enabled = false;

                    // Render stats
                    GUILayout.Label(string.Format("Current Wave: {0}", manager.CurrentWave));
                    GUILayout.Label(string.Format("Enemies Remaining: {0}", manager.InstancesRemaining));
                    GUILayout.Label(string.Format("Enemies to Spawn: {0}", manager.InstancesToSpawn));

                    GUI.enabled = old;
                }
                GUILayout.EndVertical();

                // Always repaint while in play mode
                Repaint();
            }
            else
            {
                EditorGUILayout.HelpBox("Stats for this controller will be displayed here when the game is running", MessageType.Info);
            }
        }
    }
}
