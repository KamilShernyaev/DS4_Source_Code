using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Helper class used to detect and raise networking errors.
/// </summary>
public static class NetError
{
    // Private
    private static string message = "The target method is only callable by the authorative server";

    // Methods
    /// <summary>
    /// Attempts to thrown an exception indicating that the specified networked method should only be called by the server.
    /// This method should only be called by networked code that should be server called but is called by either a local or remote client.
    /// </summary>
    public static void raise()
    {
        // Check for editor
#if UNITY_EDITOR
        // Check for play mode
        if (Application.isPlaying == false)
            return;

        Debug.LogError(message);
#endif

        // Throw the exception
        throw new InvalidOperationException(message);
    }
}
