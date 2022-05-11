using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// A spawn area is an area in 3D space defined by a trigger collider that owns one or more child spawn points.
    /// Spawn areas will only spawn an object if a specified collision is triggered. For example, The area may only spawn if the player object is within the area.
    /// This means that enemies are never too far from the player which can be especially important in large and complex maps.
    /// </summary>
    public class SpawnArea : SpawnBase, ISpawn
    {
        // Private
        private List<Collider> collidingObjects = new List<Collider>();
        private List<Collider2D> collidingObjects2D = new List<Collider2D>();

        // Public
        /// <summary>
        /// The current spawn settings used by this spawn area.
        /// </summary>
        public SpawnSettings spawnSettings = SpawnSettings.UseParent;
        /// <summary>
        /// The spawnable manager that has been linked to this spawn area.
        /// </summary>
        public SpawnableManager spawner;
        /// <summary>
        /// The spawn mode used by this spawn area.
        /// </summary>
        public SpawnMode spawnMode = SpawnMode.Random;
        /// <summary>
        /// When true, the spawn area will accept spawn requsts event if the player has not entered its trigger zone.
        /// </summary>
        public bool alwaysActive = false;
        /// <summary>
        /// The tag for the trigger object that activates and deactivates this area.
        /// </summary>
        public string areaTag = "Player";
        /// <summary>
        /// The collision layer that this area should use.
        /// </summary>
        public LayerMask collisionLayer = 1;

        // Properties
        /// <summary>
        /// Get the current spawn settings for this area.
        /// </summary>
        public override SpawnSettings SpawnSettings
        {
            get { return spawnSettings; }
        }

        /// <summary>
        /// Get the current spawnable manager associated with this spawn area.
        /// </summary>
        public override ISpawnableManager Spawner
        {
            get { return spawner; }
        }

        // Methods
        /// <summary>
        /// Called by unity on startup.
        /// </summary>
        protected override void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Find all spawn points
            findSpawns();

            base.Start();            
        }

        /// <summary>
        /// Attempt to spawn an object.
        /// </summary>
        /// <returns>The trnasform of the spawned object</returns>
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
        /// Iterates through all spawn points associated with this area and returns true if one or more of the spawn points are un-occupied and can spawn.
        /// </summary>
        /// <returns>True is there is atleast one spawn point that can spawn an object</returns>
        public override bool canSpawn()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure our spawn is correctly setup
            if (this.isValidConfiguration() == false)
                return false;

            // Check if the area is active
            if (collidingObjects.Count == 0 && collidingObjects2D.Count == 0)
                if (alwaysActive == false)
                    return false;

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
        /// Attempts to iterate through the collection of spawn points associated with this area to find a spawn point that is free.
        /// </summary>
        /// <returns>An insatnce of the spawn infor class</returns>
        public override SpawnInfo getSpawnInfo()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Select a spawn using the spawn mode
            ISpawn spawn = this.selectSpawn(spawnMode);

            // Check for error
            if (spawn == null)
                return null;

            // Get the spawn info
            return spawn.getSpawnInfo();
        }

        /// <summary>
        /// Called by unity when a collider enters this area.
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerEnter(Collider collider)
        {
            // Check for layer
            if (isLayerMasked(collider.gameObject, collisionLayer) == true)
            {
                // Make sure the collider does not already exist in the list for sme unknown reason
                if (collidingObjects.Contains(collider) == false)
                {
                    // Check for tag
                    if (string.IsNullOrEmpty(areaTag) == false)
                        if (collider.CompareTag(areaTag) == false)
                            return;

                    // Register the object as colliding with this spawn point
                    collidingObjects.Add(collider);
                }
            }
        }

        /// <summary>
        ///  Called by unity when a collider exits this area.
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerExit(Collider collider)
        {
            // Remove any colliding objects
            if (collidingObjects.Contains(collider) == true)
            {
                // Check for tag
                if (string.IsNullOrEmpty(areaTag) == false)
                    if (collider.CompareTag(areaTag) == false)
                        return;

                // Unregister the object as colliding
                collidingObjects.Remove(collider);
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            // Check for layer
            if (isLayerMasked(collider.gameObject, collisionLayer) == true)
            {
                // Make sure the collider does not already exist in the list for sme unknown reason
                if (collidingObjects2D.Contains(collider) == false)
                {
                    // Check for tag
                    if (string.IsNullOrEmpty(areaTag) == false)
                        if (collider.CompareTag(areaTag) == false)
                            return;

                    // Register the object as colliding with this spawn point
                    collidingObjects2D.Add(collider);
                }
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            // Remove any colliding objects
            if (collidingObjects2D.Contains(collider) == true)
            {
                // Check for tag
                if (string.IsNullOrEmpty(areaTag) == false)
                    if (collider.CompareTag(areaTag) == false)
                        return;

                // Unregister the object as colliding
                collidingObjects2D.Remove(collider);
            }
        }

        private bool isLayerMasked(GameObject target, LayerMask layer)
        {
            // Check for direct match
            if (target.layer == layer.value)
                return true;

            // Use bitwise comparison
            return ((layer.value & (1 << target.layer)) > 0);
        }

        private void findSpawns()
        {
            // Locate all spawn points for this area
            spawns = GetComponentsInChildren<SpawnPoint>();
        }

        /// <summary>
        /// Called by Unity to render spawn links.
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            // Draw links betweek the areas and the spawn points
            Gizmos.color = Color.yellow;

#if UNITY_EDITOR
            // Update the collection of spawns
            findSpawns();
#endif

            // Draw sphere
            Gizmos.DrawSphere(transform.position, 0.5f);

            // Iterate through each spawn
            foreach (ISpawn spawn in this)
            {
                // Draw from the spawn to the area
                Gizmos.DrawLine(transform.position, spawn.transform.position);
            }
        }
    }
}
