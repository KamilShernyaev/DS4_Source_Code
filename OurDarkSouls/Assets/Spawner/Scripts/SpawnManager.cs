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
    /// The spawn manager is the main interface for requesting an object to be spawned.
    /// It is able to work with either a series of spawn areas or spawn points depending on the settings.
    /// </summary>
    public class SpawnManager : SpawnBase, ISpawn
    {
        // Public
        /// <summary>
        /// The spawner associated with this spawn manager.
        /// </summary>
        public SpawnableManager spawner;

        /// <summary>
        /// The spawn mode that should be used. Default is random.
        /// </summary>
        public SpawnMode spawnMode = SpawnMode.Random;

        /// <summary>
        /// Should the manager use spawn areas or spawn points.
        /// </summary>
        public bool useAreas = true;

#if UNITY_EDITOR
        /// <summary>
        /// Should spawn links be rendered in the editor.
        /// </summary>
        public bool drawLinks = true;

        /// <summary>
        /// The colour to render spawn nodes in.
        /// </summary>
        public Color nodeColour = Color.magenta;

        /// <summary>
        /// The colour to render spawn links in.
        /// </summary>
        public Color linkColour = Color.magenta;
#endif

        // Properties
        /// <summary>
        /// The spawn settings used by this spawn manager.
        /// </summary>
        public override SpawnSettings SpawnSettings
        {
            get { return SpawnSettings.Custom; }
        }

        /// <summary>
        /// The spawnable manager accociated with this spawn manager.
        /// </summary>
        public override ISpawnableManager Spawner
        {
            get { return spawner; }
        }

        // Methods
        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected override void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Find all spawn points
            findSpawns();

            // Call the base method
            base.Start();
        }

        /// <summary>
        /// Attempt to spawn an item using current settings.
        /// </summary>
        /// <returns>The transform of the spawned item</returns>
        public override Transform spawn()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure we can spawn
            if (canSpawn() == false)
            {
                // Spawn failed
                invokeSpawnFailedEvent();
                return null;
            }

            // Select the spawn location
            ISpawn spawn = this.selectSpawn(spawnMode);

            // Call spawn on the child
            Transform result = spawn.spawn();

            // Trigger event
            if (result == null) invokeSpawnFailedEvent(); else invokeSpawnedEvent(result);

            return result;
        }

        /// <summary>
        /// Attempts to spawn an item at a spawn point.
        /// If successful the return value will be true otherwise false.
        /// </summary>
        /// <param name="toSpawn">The transform of the object to spawn</param>
        /// <returns>True if the spawn was successful otherwise false</returns>
        public virtual bool spawn(Transform toSpawn)
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Check if we can spawn
            if (canSpawn() == false)
            {
                // Trigger failed
                invokeSpawnFailedEvent();
                return false;
            }

            // Get the spawn point
            ISpawn spawn = this.selectSpawn(spawnMode);

            // Get the spawn info
            SpawnInfo info = spawn.getSpawnInfo();

            // Spawn the item
            info.spawnObjectAt(toSpawn);

            // Success
            invokeSpawnedEvent(toSpawn);

            return true;
        }

        /// <summary>
        /// Returns true if the spawn manager is able to spawn an item at any location otherwise false.
        /// </summary>
        /// <returns>True if there is a spawn available</returns>
        public override bool canSpawn()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure there is atleat 1 free spawn
            foreach (ISpawn spawnPoint in this)
            {
                // Check if we can spawn an object at this location
                if (spawnPoint.canSpawn() == true)
                {
                    // There is a free spawn point in this area
                    return true;
                }
            }

            // Failed to find a spawn
            return false;
        }

        /// <summary>
        /// Returns an instance of the spawn info class representing a free spawn point at the time of calling this method.
        /// This can be used to access the location and rotation of the spawn point for manual spawning.
        /// </summary>
        /// <returns>An instance of the spawn info class representing a specific spawn location</returns>
        public override SpawnInfo getSpawnInfo()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Select a spawn using the spawn mode
            ISpawn spawn = this.selectSpawn(spawnMode);

            // Check for null
            if (spawn == null)
                return null;

            // Get the spawn info
            return spawn.getSpawnInfo();
        }

        private void findSpawns()
        {
            // Locate all spawn areas in this manager
            if (useAreas == true)
            {
                // Find all areas in this manager
                spawns = GetComponentsInChildren<SpawnArea>();
            }
            else
            {
                // Find all points in this manager
                spawns = GetComponentsInChildren<SpawnPoint>();
            }
        }

        /// <summary>
        /// Called by Unity to render spawn links.
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            // Make sure we can draw
            if (drawLinks == false)
                return;

            // Draw links betweek the areas and the spawn points
            Gizmos.color = nodeColour;

            // Update the collection of spawns
            findSpawns();
#endif

            // Draw sphere
            Gizmos.DrawSphere(transform.position, 0.5f);

#if UNITY_EDITOR
            Gizmos.color = linkColour;
#endif

            // Iterate through each spawn
            foreach (ISpawn spawn in this)
            {
                // Draw from the spawn to the area
                Gizmos.DrawLine(transform.position, spawn.transform.position);
            }
        }
    }
}
