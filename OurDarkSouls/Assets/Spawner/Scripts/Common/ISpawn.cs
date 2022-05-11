using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UltimateSpawner
{
    /// <summary>
    /// How a spawn point is selected
    /// </summary>
    public enum SpawnMode
    {
        /// <summary>
        /// Objects will be spawned sequentially at all avalable spawn points.
        /// </summary>
        Sequential,
        /// <summary>
        /// Objects will be spawned at randomly selected spawn points.
        /// </summary>
        Random,
    }

    /// <summary>
    /// Which spawner should be used to create the enemy
    /// </summary>
    public enum SpawnSettings
    {
        /// <summary>
        /// The spawner should inherit its spawn settings from its parent.
        /// </summary>
        UseParent,
        /// <summary>
        /// The spawner should use its own predefined spawn settings.
        /// </summary>
        Custom,
    }

    // Delegates
    /// <summary>
    /// The general delaget used by spawnerd to indicate that an item has been spawned.
    /// </summary>
    /// <param name="item">The transform of the item that was spawned</param>
    public delegate void SpawnDelegate(Transform item);

    /// <summary>
    /// The standard interface for all spawner classes
    /// </summary>
    public interface ISpawn : IEnumerable<ISpawn>
    {
        // Properties
        /// <summary>
        /// Get the current spawn settings for this spawner instance.
        /// </summary>
        SpawnSettings SpawnSettings { get; }

        /// <summary>
        /// Get the parent spawn instance of this object or null if this spawn instance is the root.
        /// </summary>
        ISpawn Parent { get; }

        /// <summary>
        /// Get the spawnable manager associated with this spawner. If null, then any calls to <see cref="spawn"/> may fail.
        /// </summary>
        ISpawnableManager Spawner { get; }

        /// <summary>
        /// Get the transform of this spawner.
        /// </summary>
        Transform transform { get; } // Implemented by Unity :-)

        // Methods
        /// <summary>
        /// Attempt to spawn an object using the current spawn settings and spawnable manager.
        /// </summary>
        /// <returns>The transform of the object that was spawned</returns>
        Transform spawn();

        /// <summary>
        /// Returns true if the spawner is able to spawn based on the current settings. This will only return false if all spawn points are occupied.
        /// </summary>
        /// <returns>True if the spawner is able to spawn an object</returns>
        bool canSpawn();

        /// <summary>
        /// Gets the spawn info for the current spawner. This method may result in delegations to child spawners.
        /// </summary>
        /// <returns>The spawn info for an appropriate spawner found using the current settings</returns>
        SpawnInfo getSpawnInfo();
    }
}