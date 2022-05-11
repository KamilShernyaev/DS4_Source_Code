using UnityEngine;
using System.Collections;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// Common base class for all spawn controllers that provided the unique spawning behaviour (E.g. wave spawning)
    /// </summary>
#if ULTIMATE_SPAWNER_NETWORKED == true
    public abstract class Controller : NetworkBehaviour
#else
    public abstract class Controller : MonoBehaviour
#endif
    {
        // Public
        /// <summary>
        /// The spawner used to spawn objects.
        /// </summary>
        [Tooltip("A reference to the spawn manager for enemy spawn points")]
        public SpawnBase spawnManager;

        // Methods
        /// <summary>
        /// When overidden this method should trigger the controller to start spawning
        /// </summary>
        public abstract void startSpawning();

        /// <summary>
        /// When overidden this method should trigger the controller to stop spawning
        /// </summary>
        public abstract void stopSpawning();
    }
}
