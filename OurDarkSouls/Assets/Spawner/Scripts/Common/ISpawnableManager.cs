using UnityEngine;
using System.Collections;

namespace UltimateSpawner
{
    /// <summary>
    /// The public interface used for all spawnable managers.
    /// A spawnable manager is responsible for selecting and spawning an appropriate prefab based on its own rules and should then spawn this instance using its assinged spawner.
    /// </summary>
    public interface ISpawnableManager
    {
        // Properties
        /// <summary>
        /// When overidden should return the number of enemies that are still alive in the scene
        /// </summary>
        int InstancesRemaining { get; }

        // Methods
        /// <summary>
        /// When overidden should return an instance of an enemy object that will be spawned into the game.
        /// </summary>
        /// <returns>A reference to an enemy game object</returns>
        GameObject createSpawnable();

        /// <summary>
        /// When overriden should remove the instance from the list of tracked objects so that the number of enemies remaining can be calulcated
        /// </summary>
        /// <param name="instance">An instance of the enemy object to destroy</param>
        /// <param name="destroyObject">Should the target object be destroyed by the spawnable manager</param>
        void spawnableDestroyed(GameObject instance, bool destroyObject);

        /// <summary>
        /// Called when the enemy manager is required to start from fresh
        /// </summary>
        void reset();
    }
}