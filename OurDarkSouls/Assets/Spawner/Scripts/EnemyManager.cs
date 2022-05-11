using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;



#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

#if ULTIMATE_SPAWNER_POOLED == true
using UltimatePooling;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// The enemy manager class is the main class that handles the creation of enemies.
    /// This class can be inherited to add pooling support if required.
    /// </summary>
    public class EnemyManager : SpawnableManager
    {
        // Private
        private List<GameObject> enemyList = new List<GameObject>();

        // Public
        /// <summary>
        /// An array of <see cref="SpawnableInfo"/> instances each representing a potential enemy that can be spawned.
        /// </summary>
        [Tooltip("The array of enmies belonging to this enemy manager")]
        public SpawnableInfo[] enemies;

        // Properties
        /// <summary>
        /// Get the number of remaining objects.
        /// </summary>
        public override int InstancesRemaining
        {
            get { return enemyList.Count; }
        }

        // Methods
        /// <summary>
        /// Attempts to create an instance of a spawnable object.
        /// </summary>
        /// <returns></returns>
        public override GameObject createSpawnable()
        {
            // Only allow the server to spawn
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Select the enemy type for spawning
            SpawnableInfo info = selectEnemy();

            // Check for error
            if (info == null)
                return null;

            // Spawn the enemy
#if ULTIMATE_SPAWNER_POOLED == true
            // Reuse a pooled instance from the pool
            GameObject instance = UltimatePool.spawn(info.prefab);
#else
            // Create the instance
            GameObject instance = Instantiate(info.prefab);
#endif

            // Spawn the instance on remote clients
#if ULTIMATE_SPAWNER_NETWORKED == true
            // Spawn on server
            NetworkServer.Spawn(instance);
#endif

            // Store the enemy in a list
            enemyList.Add(instance);

            return instance;
        }

        /// <summary>
        /// Use this method to inform the enemy manager that an enemy has died.
        /// </summary>
        /// <param name="instance">A refernce to the enemy instance</param>
        [Obsolete("This method has been depreciated as of version 1.2.0 and you should use the overloaded method wih the additional bool parameter")]
        public void spawnableDestroyed(GameObject instance)
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure we are destroying an enemy
            if (enemyList.Contains(instance) == true)
            {
                // Remove from the list
                enemyList.Remove(instance);
            }
        }

        /// <summary>
        /// Inform the enemy manager that an enemy has died.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="destroyObject"></param>
        public override void spawnableDestroyed(GameObject instance, bool destroyObject)
        {
            // Make sure the server is handling the destroyed event
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure we are destroying an enemy
            if(enemyList.Contains(instance) == true)
            {
                // Do we have authority to destroy the instance
                if(destroyObject == true)
                {
#if ULTIMATE_SPAWNER_NETWORKED == true
                    // Destroy on remote clients
                    NetworkServer.Destroy(instance);
#endif

                    // Destroy on local client
#if ULTIMATE_SPAWNER_POOLED == true
                    // Return the instance to the pool for reuse
                    UltimatePool.despawn(instance);
#else
                    // Destroy the instance
                    Destroy(instance);
#endif
                }

                // Remove from list
                enemyList.Remove(instance);
            }
        }

        /// <summary>
        /// Selects an appropraite enemyinfo from the collection based on the current settings.
        /// </summary>
        /// <returns>An appropriate enemyinfo</returns>
        public virtual SpawnableInfo selectEnemy()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Check for empty enemies
            if (enemies.Length == 0)
            {
                Debug.LogError("Failed to spawn an enemy because the enemy manager does not contain any enemies");
                return null;
            }

            float accumulator = 0;

            // FInd the total spawn chance value
            foreach (SpawnableInfo info in enemies)
                accumulator += info.spawnChance;

            // Select a random value
            float value = Random.Range(0, accumulator);

            // Reset the accumulator
            accumulator = 0;

            // Find the selected enemy
            foreach (SpawnableInfo info in enemies)
            {
                // Add to the accumulator
                accumulator += info.spawnChance;

                // Check if we have found the best match
                if (value < accumulator)
                    return info;
            }

            // Default index
            return enemies[0];
        }

        /// <summary>
        /// Stops tracking enemies that are still alive.
        /// </summary>
        public override void reset()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            enemyList.Clear();
        }

        /// <summary>
        /// Use this method to inform all enemy managers that an enemy has died.
        /// Note that this will only affect the enemy manager that spawned the enemy.
        /// </summary>
        /// <param name="instance"></param>
        [Obsolete("This method has been depreciated. Instead you should use 'SpawnableManager.informSpawnableDestroyed'")]
        public static void informEnemyDeath(GameObject instance)
        {
            // Get all managers
            EnemyManager[] managers = Component.FindObjectsOfType<EnemyManager>();

            // Inform each manager
            foreach (EnemyManager manager in managers)
            {
                // Pass the destroyed object to the manager
                manager.spawnableDestroyed(instance);
            }
        }

        /// <summary>
        /// This method will get the number of enemies that are currently alive from all valid enemy managers.
        /// </summary>
        /// <returns>The number of alive enemies</returns>
        [Obsolete("This method has been depreciated. Instead you should use 'SpawnableManager.SpawnablesRemaining'")]
        public static int enemiesRemaining()
        {
            int count = 0;

            // Get all managers
            EnemyManager[] managers = Component.FindObjectsOfType<EnemyManager>();

            // Inform each manager
            foreach (EnemyManager manager in managers)
            {
                // Pass the destroyed object to the manager
                count += manager.InstancesRemaining;
            }

            return count;
        }
    }
}
