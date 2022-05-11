using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

namespace UltimateSpawner
{
    /// <summary>
    /// Represents a specific element in the enemies array of the enemy manager script.
    /// Specifies which enemy prefab to use as well as how likley the enemy is to spawn based on a weighting value.
    /// </summary>
    [Serializable]
    public class SpawnableInfo
    {
        // Public
        /// <summary>
        /// The prefab associated with this enenemy.
        /// </summary>
        [Tooltip("A reference to the prefab to spawn")]
        public GameObject prefab;

        /// <summary>
        /// The chance value used to determine how likley the enemy is to spawn.
        /// </summary>
        [Range(0, 1)]
        [Tooltip("How likley is the enemy to be spawned (The higher the value the more likley of spawning the enemy)")]
        public float spawnChance = 0.5f;
    }
}
