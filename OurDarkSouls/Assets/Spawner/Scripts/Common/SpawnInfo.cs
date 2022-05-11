using UnityEngine;
using System;
using System.Collections;

namespace UltimateSpawner
{
    /// <summary>
    /// Represents a location and rotation in 3D space where an object can be spawned.
    /// The class also maintains a reference to the spawn point that created it.
    /// </summary>
    [Serializable]
    public sealed class SpawnInfo
    {
        // Private
        private ISpawn owner = null;

        private Vector3 spawnPoint = Vector3.zero;
        private Quaternion spawnRotation = Quaternion.identity;

        // Properties
        /// <summary>
        /// The spawner that created this spawn info.
        /// </summary>
        public ISpawn SpawnPoint
        {
            get { return owner; }
        }

        /// <summary>
        /// The location in 3D space representing where an object should be spawned.
        /// </summary>
        public Vector3 SpawnLocation
        {
            get { return spawnPoint; }
        }

        /// <summary>
        /// The initial rotation to spawn an object with.
        /// </summary>
        public Quaternion SpawnRotation
        {
            get { return spawnRotation; }
        }

        // Constructor
        /// <summary>
        /// Create a new instance of the spawn info using a spawn location.
        /// </summary>
        /// <param name="owner">The spawn location that created this instance</param>
        /// <param name="transform">The transform representing the location and rotation</param>
        public SpawnInfo(ISpawn owner, Transform transform)
        {
            // Store the owner of this spawn point
            this.owner = owner;

            // Get the transform information
            this.spawnPoint = transform.position;
            this.spawnRotation = transform.rotation;
        }

        // Methods
        /// <summary>
        /// Create a new instance of the spawn info using location.
        /// </summary>
        /// <param name="toSpawn">The transform representing the location and rotation</param>
        public void spawnObjectAt(Transform toSpawn)
        {
            // Apply the transform
            toSpawn.position = spawnPoint;
            toSpawn.rotation = spawnRotation;
        }
    }
}