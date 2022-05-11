using UnityEngine;
using System;
using System.Collections;
using Action = System.Action;
using Random = UnityEngine.Random;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// Represents a spawn controller that spawns for either a preset amount of time or indefinitley.
    /// </summary>
    public class InfiniteController : Controller
    {
        // Event
        /// <summary>
        /// Triggered when the spawner is set to spawn for a fixed time and that time has elapsed
        /// </summary>
        public event Action onSpawnerEnd;

        // Private
        private float lastTime = 0;

        // Public
        /// <summary>
        /// When true, spawning will automatically be triggered at startup.
        /// </summary>
        [Tooltip("Should we begin spawning on start")]
        public bool playOnStart = true;


        /// <summary>
        /// The minimum number of enemies that should be in the scene at any time. The spawner will always try to keep the current number of enemies above or equal to this number.
        /// </summary>
        [Tooltip("The minimum number of enemies that should exist at any time")]
        public int minimumSpawnCount = 4;

        /// <summary>
        /// The maximum number of enemies that should be in the scene at any time. The spawner will never spawn if the current enemy count is above or equal to this number.
        /// </summary>
        [Tooltip("The maximum number of enemies that should exist at any time")]
        public int maximumSpawnCount = 12;

        /// <summary>
        /// The amount of time in seconds that the controller must wait between spawn calls.
        /// </summary>
        [Tooltip("The amount of time in seconds to wait before the next spawn")]
        public float spawnDelay = 0.3f;

        /// <summary>
        /// When true, the spawner will only spawn for a preset amount of time and then stop spawning.
        /// </summary>
        [Tooltip("Should the spawner continue spawning for a set time or infinatley")]
        public bool stopAfterTime = false;

        /// <summary>
        /// The amount of time in seconds that the spawner should be enabled for. Once the time has expired then the spawner will stop spawning.
        /// </summary>
        [Tooltip("The time in seconds that the spawner should wait before halting spawning")]
        public float stopAfter = 26;

        // Methods
        private void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Launch spawn routine immediatley
            if (playOnStart == true)
                startSpawning();
        }

        /// <summary>
        /// Begin infinatley spawning enemies using the current settings
        /// </summary>
        public override void startSpawning()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Start spawning
            StartCoroutine(spawnRoutine());
        }

        /// <summary>
        /// Stop spawning anymore enemies
        /// </summary>
        public override void stopSpawning()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Stop spawning
            StopCoroutine(spawnRoutine());
        }

        private IEnumerator spawnRoutine()
        {
            // Store the start time
            lastTime = Time.time;

            // Delayed start
            yield return new WaitForSeconds(1);

            // Loop forever
            while (true)
            {
                // Check if we need to exit
                if (stopAfterTime == true)
                {
                    // CHeck if enough time has passed
                    if (lastTime + stopAfter > Time.time)
                    {
                        // We need to stop spawning now
                        break;
                    }
                }                

                // Check for less than required
                if (spawnManager.Spawner.InstancesRemaining < minimumSpawnCount)
                {
                    // Wait for spawn delay
                    yield return new WaitForSeconds(spawnDelay);

                    // Spawn an enemy
                    yield return StartCoroutine(spawnImmediate());
                }

                // Calculate the chance of spawning
                int value = maximumSpawnCount - minimumSpawnCount;

                if (Random.Range(0, value) > value / 2)
                {
                    // Spawn an enemy
                    yield return StartCoroutine(spawnImmediate());
                }

                if (spawnManager.Spawner.InstancesRemaining >= maximumSpawnCount)
                {
                    yield return new WaitForSeconds(2);
                }

                yield return new WaitForSeconds(0.4f);
            }

            // Trigger the even on the way out
            if (onSpawnerEnd != null)
                onSpawnerEnd();
        }

        private IEnumerator spawnImmediate()
        {
            bool canSpawn = false;

            // Block until we can spawn
            while (canSpawn == false)
            {
                // Check if we can spawn
                canSpawn = spawnManager.canSpawn();
                yield return null;
            }

            // Spawn the enemy 
            spawnManager.spawn();
        }
    }
}
