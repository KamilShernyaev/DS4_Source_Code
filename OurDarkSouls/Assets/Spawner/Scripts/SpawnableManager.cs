using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// This base class acts as an interface and allows you to implement your own spawning system by imeplemtning the abstract methods.
    /// You can then use your derived class with the wave manager whcihc will make calls to spawn enemies when required.
    /// This should really be an interface but it need to be exposed to the editor and Unity wont play well.
    /// </summary>
#if ULTIMATE_SPAWNER_NETWORKED == true
    public abstract class SpawnableManager : NetworkBehaviour, ISpawnableManager
#else
    public abstract class SpawnableManager : MonoBehaviour, ISpawnableManager
#endif
    {
        // Private
        private static List<SpawnableManager> cachedManagers = new List<SpawnableManager>();

        // Properties
        /// <summary>
        /// When overidden should return the number of enemies that are still alive in the scene
        /// </summary>
        public abstract int InstancesRemaining { get; }

        /// <summary>
        /// This property will get the total number of spawnables that are currently alive.
        /// This property will search all active managers and is a direct replacement for 'EnemyManager.enemiesRemaining'.
        /// </summary>
        public static int SpawnablesRemaining
        {
            get
            {
                int count = 0;

                // Count the remaining spawnables for each manager
                foreach (SpawnableManager manager in cachedManagers)
                {
                    // Pass the destroyed object instance to the manager
                    count += manager.InstancesRemaining;
                }

                return count;
            }
        }

        // Methods
        /// <summary>
        /// When overidden should return an instance of an enemy object that will be spawned into the game.
        /// </summary>
        /// <returns>A reference to an enemy game object</returns>
        public abstract GameObject createSpawnable();

        /// <summary>
        /// When overriden should remove the instance from the list of tracked objects so that the number of enemies remaining can be calulcated
        /// </summary>
        /// <param name="instance">An instance of the enemy object to destroy</param>
        /// <param name="destroyObject">Should the spawnable manager destory the object</param>
        public abstract void spawnableDestroyed(GameObject instance, bool destroyObject);


        /// <summary>
        /// Called when the enemy manager is required to start from fresh
        /// </summary>
        public abstract void reset();

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void OnEnable()
        {
            // Register this spawnable manager 
            cachedManagers.Add(this);
        }

        /// <summary>
        /// Called by Unity
        /// </summary>
        protected virtual void OnDisable()
        {
            // Un-register this instance
            cachedManagers.Remove(this);
        }

        /// <summary>
        /// Use this method to inform all spawnable managers that a spawned instance is about to be destroyed.
        /// This method is a direct replacement for 'EnemyManager.informEnemyDeath' and allows the enemy spawner to be decoupled.
        /// Note that only the spawnable manager that created the instance will act as a result of this call.
        /// </summary>
        /// <param name="instance">The spawned instance that is about to be destroyed</param>
        /// <param name="destroyObject">When true, the manager that created the instance will be responsible for destroying the instance</param>
        public static void informSpawnableDestroyed(GameObject instance, bool destroyObject = true)
        {
            // Check for error
            if (instance == null)
                return;

            // Inform each manager of the death
            foreach (SpawnableManager manager in cachedManagers)
            {
                // Pass the destroyed object instance to the manager
                manager.spawnableDestroyed(instance, destroyObject);
            }
        }
    }
}
