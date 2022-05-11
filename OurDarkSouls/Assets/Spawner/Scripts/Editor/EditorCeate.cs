using UnityEngine;
using System.Collections;
using UnityEditor;

using UltimateSpawner;

public static class EditorCeate
{ 
    [MenuItem("Tools/Ultimate Spawner/Create/Spawn Point", priority = 1)]
    public static void createSpawnPoint()
    {
        GameObject parent = Selection.activeGameObject;

        // Create the spawn point
        GameObject go = new GameObject("Spawn Point");
        go.AddComponent<SphereCollider>().isTrigger = true;
        go.AddComponent<SpawnPoint>();

        // Try to add parent
        if (parent != null)
            go.transform.SetParent(parent.transform);

        // Update selection
        Selection.activeGameObject = go;

        // Register for undo
        Undo.RegisterCreatedObjectUndo(go, "Create Spawn Point");
    }

    [MenuItem("Tools/Ultimate Spawner/Create/Spawn Area", priority = 2)]
    public static void createSpawnArea()
    {
        GameObject parent = Selection.activeGameObject;

        // Create the spawn area
        GameObject go = new GameObject("Spawn Area");
        go.AddComponent<BoxCollider>().isTrigger = true;
        go.AddComponent<SpawnArea>();

        // Try to add parent
        if (parent != null)
            go.transform.SetParent(parent.transform);

        // Update selection
        Selection.activeGameObject = go;

        // Regsiter for undo
        Undo.RegisterCompleteObjectUndo(go, "Create Spawn Area");
    }

    [MenuItem("Tools/Ultimate Spawner/Create/Spawn Manager", priority = 3)]
    public static void createSpawnManager()
    {
        GameObject parent = Selection.activeGameObject;

        // Create the spawn area
        GameObject go = new GameObject("Spawn Manager");
        go.AddComponent<SpawnManager>();

        // Try to add parent
        if (parent != null)
            go.transform.SetParent(parent.transform);

        // Update selection
        Selection.activeGameObject = go;

        // Regsiter for undo
        Undo.RegisterCompleteObjectUndo(go, "Create Spawn Manager");
    }

    [MenuItem("Tools/Ultimate Spawner/Create/Wave Controller", priority = 21)]
    public static void createWaveController()
    {
        GameObject parent = Selection.activeGameObject;

        // Create the spawn area
        GameObject go = new GameObject("Wave Controller");
        go.AddComponent<WaveController>();

        // Try to add parent
        if (parent != null)
            go.transform.SetParent(parent.transform);

        // Update selection
        Selection.activeGameObject = go;

        // Regsiter for undo
        Undo.RegisterCompleteObjectUndo(go, "Create Wave Controller");
    }

    [MenuItem("Tools/Ultimate Spawner/Create/Infinite Controller", priority = 22)]
    public static void createInfiniteController()
    {
        GameObject parent = Selection.activeGameObject;

        // Create the spawn area
        GameObject go = new GameObject("Infinite Controller");
        go.AddComponent<InfiniteController>();

        // Try to add parent
        if (parent != null)
            go.transform.SetParent(parent.transform);

        // Update selection
        Selection.activeGameObject = go;

        // Regsiter for undo
        Undo.RegisterCompleteObjectUndo(go, "Create Infinite Controller");
    }

    [MenuItem("Tools/Ultimate Spawner/Documentation")]
    public static void showDocumentation()
    {
        Application.OpenURL(Application.dataPath + "/Ultimate Spawner/UltimateSpawner_UserGuide.pdf");
    }

    [MenuItem("Tools/Ultimate Spawner/Scripting Reference")]
    public static void showScriptingReference()
    {
        Application.OpenURL(Application.dataPath + "/Ultimate Spawner/UltimateSpawner_ScriptingReference.chm");
    }
}
