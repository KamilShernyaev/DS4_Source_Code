using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// A spawn point is a point in 3D spawn that represents a location that certain game objects can be spawned within the level.
    /// In oreder to avoid overlapping objects and unpredictable phyisics behaviour, the spawn point must first be un-occupied before an item can be spawned.
    /// </summary>
    public class SpawnPoint : SpawnBase
    {
        // Enum
        /// <summary>
        /// The method used to detect whether the spawn point is occupied;
        /// </summary>
        public enum OccupiedCheckMode
        {
            /// <summary>
            /// Use a sphere overlap to detect nearby objects.
            /// </summary>
            OverlapSphere,
            /// <summary>
            /// Use a overlap sphere to detect nearby objects using the 2D physics system.
            /// </summary>
            OverlapSphere2D,
            /// <summary>
            /// Use the trigger events of the object.
            /// </summary>
            TriggerEnterExit,
        }

        // Private
        private List<Collider> collidingObjects = new List<Collider>();
        private List<Collider2D> collidingObjects2D = new List<Collider2D>();
        private SpawnInfo cachedInfo = null;

        // Public
        /// <summary>
        /// The spawn settings used by this spawn point.
        /// </summary>
        public SpawnSettings spawnSettings = SpawnSettings.UseParent;

        /// <summary>
        /// The spawnable manager associated with this spawn point. 
        /// </summary>
        public SpawnableManager spawner;

        /// <summary>
        /// Should the spawn point check if it is occupied before spawning.
        /// </summary>
        public bool performOccupiedCheck = true;

        /// <summary>
        /// The radius that the spawn point occupies. Used in occupied checks.
        /// </summary>
        public float spawnRadius = 1;

        /// <summary>
        /// The mode used to detect whether the spawn point is occupied.
        /// </summary>
        public OccupiedCheckMode occupiedCheckMode = OccupiedCheckMode.OverlapSphere;

        /// <summary>
        /// The collision layer to check for collisions on.
        /// </summary>
        public LayerMask collisionLayer = 0;

#if UNITY_EDITOR
        /// <summary>
        /// The colour that the collider is rendered in.
        /// </summary>
        public Color colliderColour = Color.green;

        /// <summary>
        /// The colour that the direction is rendered in.
        /// </summary>
        public Color directionColour = Color.blue;
#endif

        // Properties
        /// <summary>
        /// Returns true if the spawn point is un-occupied.
        /// </summary>
        public bool IsSpawnFree
        {
            get { return canSpawn(); }
        }

        /// <summary>
        /// The spawn setting used by this spawn point.
        /// </summary>
        public override SpawnSettings SpawnSettings
        {
            get { return spawnSettings; }
        }

        /// <summary>
        /// The spawnable manager accociated with this spawn point.
        /// </summary>
        public override ISpawnableManager Spawner
        {
            get { return spawner; }
        }

        // Methods
        /// <summary>
        /// Attempt to spawn an item using the current settings.
        /// </summary>
        /// <returns>The transform of the spawned item</returns>
        public override Transform spawn()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure we can spawn
            if (IsSpawnFree == false)
            {
                // Spawn failed
                invokeSpawnFailedEvent();
                return null;
            }

            // Create the spawner object
            Transform instance = this.createSpawnableInstance();

            // Check for error
            if (instance == null)
            {
                // Spawn failed
                invokeSpawnFailedEvent();
                return null;
            }

            // Get the spawn info
            SpawnInfo info = getSpawnInfo();

            // Spawn the item
            info.spawnObjectAt(instance);

            // Success
            invokeSpawnedEvent(instance);

            return instance;
        }

        /// <summary>
        /// Is the spawn point able to spawn an item.
        /// </summary>
        /// <returns>True if the spawn point can spawn an item</returns>
        public override bool canSpawn()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Make sure our spawn is correctly setup
            if (this.isValidConfiguration() == false)
                return false;

            // Check for trival case
            if (performOccupiedCheck == false)
                return true;

            // Allow the user to specify how occupied spawns are detected
            switch (occupiedCheckMode)
            {
                // Use physics overlap sphere to validate the spawn
                default:
                case OccupiedCheckMode.OverlapSphere:
                    {
                        // Get the spawn info
                        SpawnInfo info = getSpawnInfo();

                        // Find the center point
                        Vector3 center = info.SpawnLocation + new Vector3(0, spawnRadius, 0);

                        // Perform the sphere overlap
                        Collider[] colliders = Physics.OverlapSphere(center, spawnRadius, 1);

                        // Iteratre through each collider
                        foreach (Collider collider in colliders)
                        {
                            // The spawn is occupied
                            if (collider.isTrigger == false)
                                return false;
                        }
                    }
                    break;

                case OccupiedCheckMode.OverlapSphere2D:
                    {
                        // Get the spawn info
                        SpawnInfo info = getSpawnInfo();

                        // Find the center point
                        Vector3 center = info.SpawnLocation + new Vector3(0, spawnRadius, 0);

                        // Perform the overlap sphere
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, spawnRadius, 1);

                        // Iterate though each collider
                        foreach(Collider2D collider in colliders)
                        {
                            // The spawn is occupied
                            if (collider.isTrigger == false)
                                return false;
                        }
                    }
                    break;

                case OccupiedCheckMode.TriggerEnterExit:
                    {
                        // Check if the collision list contains any items
                        if (collidingObjects.Count > 0 || collidingObjects2D.Count > 0)
                        {
                            // This spawn point is occupied
                            return false;
                        }
                    }
                    break;
            }

            // The spawn point is not occupied and we can spawn at this location
            return true;
        }

        /// <summary>
        /// Get the spawn info for this spawn point.
        /// </summary>
        /// <returns>An instance of the spawn info class representing this spawn points location and orientation</returns>
        public override SpawnInfo getSpawnInfo()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Check for cached info
            if (cachedInfo != null)
                return cachedInfo;

            // Create a new spawn info
            cachedInfo = new SpawnInfo(this, transform);

            // Get the info
            return cachedInfo;
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerEnter(Collider collider)
        {
            // Exclude triggers
            if (collider.isTrigger == true)
                return;

            // Check for layer
            if (isLayerMasked(collider.gameObject, collisionLayer) == true)
            {
                // Make sure the collider does not already exist in the list for sme unknown reason
                if (collidingObjects.Contains(collider) == false)
                {
                    // Register the object as colliding with this spawn point
                    collidingObjects.Add(collider);
                }
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerExit(Collider collider)
        {
            // Exclude triggers
            if (collider.isTrigger == true)
                return;

            // Remove any colliding objects
            if (collidingObjects.Contains(collider) == true)
            {
                // Unregister the object as colliding
                collidingObjects.Remove(collider);
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            // Exclude triggers
            if (collider.isTrigger == true)
                return;

            // Check for layer
            if (isLayerMasked(collider.gameObject, collisionLayer) == true)
            {
                // Make sure the collider does not already exist in the list for sme unknown reason
                if (collidingObjects2D.Contains(collider) == false)
                {
                    // Register the object as colliding with this spawn point
                    collidingObjects2D.Add(collider);
                }
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            // Exclude triggers
            if (collider.isTrigger == true)
                return;

            // Remove any colliding objects
            if (collidingObjects2D.Contains(collider) == true)
            {
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

        /// <summary>
        /// Called by Unity to render spawn links.
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            // Get the spawn info
            SpawnInfo info = getSpawnInfo();

#if UNITY_EDITOR
            // Force recalualte the cache spawn info
            info = new SpawnInfo(this, transform);
#endif

            // Find the center point
            Vector3 center = info.SpawnLocation + new Vector3(0, spawnRadius, 0);

#if UNITY_EDITOR
            // Use green for the spawn area
            Gizmos.color = colliderColour;
#endif
            // Draw 3d sphere
            Gizmos.DrawWireSphere(center, spawnRadius);

            // Draw the facing direction arrow
#if UNITY_EDITOR
            Gizmos.color = directionColour;
#endif
            Gizmos.DrawRay(center, transform.forward * 2);
        }
    }
}
