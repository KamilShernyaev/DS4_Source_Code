using UnityEngine;
using System.Collections;

namespace UltimateSpawner.Demo
{
    /// <exclude/>
    public class PlayerSpawner : MonoBehaviour
    {
        // Public
        public GameObject playerInstance;

        // Methods
        private void Start()
        {
            respawnPlayer();
        }

        public void respawnPlayer()
        {
            // Get the spawn manager for the player
            SpawnManager manager = GetComponent<SpawnManager>();

            // CHeck for error
            if (manager == null)
            {
                Debug.LogError("Cannot spawn player becuase the player spawn manager could not be found");
                return;
            }

            // Check for player instance
            if (playerInstance == null)
            {
                Debug.LogError("Cannot spawn player because the player instance has not been assigned");
                return;
            }

            // Spawn the player
            manager.spawn(playerInstance.transform);
        }
    }
}