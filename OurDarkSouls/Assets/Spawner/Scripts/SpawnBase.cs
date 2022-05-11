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
    /// The base class for spawn classes that should trigger events.
    /// </summary>
#if ULTIMATE_SPAWNER_NETWORKED == true
    public abstract class SpawnBase : NetworkBehaviour, ISpawn    
#else
    public abstract class SpawnBase : MonoBehaviour, ISpawn
#endif
    {
        // Events
        /// <summary>
        /// Called when an object has been spawned by this spawner.
        /// </summary>
        public event SpawnDelegate onObjectSpawned;
        /// <summary>
        /// Called when this spawner failed to spawn an object.
        /// </summary>
        public event Action onObjectSpawnFailed;

        // Protected
        /// <summary>
        /// The child spawn locations accociated with this spawn location.
        /// </summary>
        protected ISpawn[] spawns = null;
        /// <summary>
        /// The parent spawn location.
        /// </summary>
        protected ISpawn parent = null;

        // Properties
        /// <summary>
        /// The spawn settings used by this spawner;
        /// </summary>
        public abstract SpawnSettings SpawnSettings { get; }

        /// <summary>
        /// The spawn manager used by this spawner.
        /// </summary>
        public abstract ISpawnableManager Spawner { get; }

        /// <summary>
        /// The parent spawn location.
        /// </summary>
        public ISpawn Parent
        {
            get { return parent; }
        }

        // Methods
        /// <summary>
        /// Attempt to generate a spawn location using current settings.
        /// </summary>
        /// <returns>A transform representing a free spawn locaiton</returns>
        public abstract Transform spawn();

        /// <summary>
        /// Attemtp to get a spawn info instance representing a spawn location.
        /// </summary>
        /// <returns>A spawn info instance representing a free spawn location</returns>
        public abstract SpawnInfo getSpawnInfo();

        /// <summary>
        /// Is the spawner able to spawn an object.
        /// </summary>
        /// <returns>True if this spawn location is able to spawn</returns>
        public abstract bool canSpawn();

        /// <summary>
        /// Called when an object has been spawned by this spawn location.
        /// </summary>
        /// <param name="transform"></param>
        public virtual void onSpawned(Transform transform) { }

        /// <summary>
        /// Called when a spawn attempt fails.
        /// </summary>
        public virtual void onSpawnFailed() { }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void Start()
        {
            // Check for parent
            if (transform.parent == null)
                return;

            // Get the parent object
            GameObject parentObject = transform.parent.gameObject;

            // Get the ISpawn parent
            parent = parentObject.GetComponent<ISpawn>();
        }


#region Event Triggers
        /// <summary>
        /// Responsible for triggering the appropriate events when a spawn was successful.
        /// </summary>
        /// <param name="target">The item that was spawned</param>
        protected void invokeSpawnedEvent(Transform target)
        {
            // Call the virtual method
            onSpawned(target);

            // Trigger the event
            if (onObjectSpawned != null)
                onObjectSpawned(target);
        }

        /// <summary>
        /// Responsible for triggering the appropriate events when a spawn was unsuccessful.
        /// </summary>
        protected void invokeSpawnFailedEvent()
        {
            // Call the virtual method
            onSpawnFailed();

            // Trigger the event
            if (onObjectSpawnFailed != null)
                onObjectSpawnFailed();
        }
#endregion

#region IEnumerable Implementation
        /// <summary>
        /// IEnumerable implementation.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ISpawn> GetEnumerator()
        {
            // Iterate through the array of spawn points
            foreach (ISpawn spawn in spawns)
                yield return spawn;
        }

        /// <summary>
        /// IEnumerable implementation.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // Iterate through the array of spawn points
            foreach (ISpawn spawn in spawns)
                yield return spawn;
        }
#endregion
    }
}
