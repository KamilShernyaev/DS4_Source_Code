
#pragma warning disable 0219
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UltimateSpawner
{
    /// <summary>
    /// An extensions class that provides common functionality for the ISpawn interface such as enumerating free spawn points.
    /// </summary>
    public static class ISpawn_Extensions
    {
        // Private
        private static Dictionary<ISpawn, IEnumerator<ISpawn>> sequential = new Dictionary<ISpawn, IEnumerator<ISpawn>>();

        // Methods
        /// <summary>
        /// Iterates through all child spawns in a specified spawn and returns a collection of un-occupied spawns.
        /// </summary>
        /// <param name="inSpawn">Extension input</param>
        /// <returns></returns>
        public static IEnumerable<ISpawn> freeSpawns(this ISpawn inSpawn)
        {
            // Iterate through the spawn to check if there are any free spawns
            foreach (ISpawn spawn in inSpawn)
            {
                // Check if the spawn is occupied
                if (spawn.canSpawn() == true)
                {
                    // Get the item
                    yield return spawn;
                }
            }
        }

        /// <summary>
        /// Iterates through all child spawns and selects a random un-occupied spawn.
        /// </summary>
        /// <param name="inSpawn">Extension input</param>
        /// <returns></returns>
        public static ISpawn randomSpawn(this ISpawn inSpawn)
        {
            // Find all free spawns
            IEnumerable<ISpawn> spawns = inSpawn.freeSpawns();

            // Get the size
            int size = spawns.size();

            // Check for error
            if (size == 0)
                return null;

            // Select a random index
            int index = Random.Range(0, size);

            // Get the spawn
            return spawns.element(index);
        }

        /// <summary>
        /// Selects a spawn based on the spawn mode from the collection of child spawns that are available.
        /// </summary>
        /// <param name="inSpawn">Extension input</param>
        /// <param name="spawnMode">The spawn mode to use for selection</param>
        /// <returns></returns>
        public static ISpawn selectSpawn(this ISpawn inSpawn, SpawnMode spawnMode)
        {
            // Different spawn modes
            switch (spawnMode)
            {
                default:
                case SpawnMode.Random:
                    {
                        // Select a random spawn (Area or point)
                        return inSpawn.randomSpawn();
                    }

                case SpawnMode.Sequential:
                    {
                        // Select the next spawn (Area or point)
                        return inSpawn.sequentialSpawn();
                    }
            }
        }

        /// <summary>
        /// Create an instance for the current spawner.
        /// </summary>
        /// <param name="inSpawn">The spawn point to spawn from</param>
        /// <returns>The trnsform of the spawned instance</returns>
        public static Transform createSpawnableInstance(this ISpawn inSpawn)
        {
            Transform result = null;

            switch (inSpawn.SpawnSettings)
            {
                case SpawnSettings.Custom:
                    {
                        // Make sure our spawner is setup
                        if (inSpawn.Spawner == null)
                            return null;

                        // Try to spawn the enemy
                        GameObject temp = inSpawn.Spawner.createSpawnable();

                        if (temp != null)
                            result = temp.transform;
                    }
                    break;

                case SpawnSettings.UseParent:
                    {
                        // Make sure our parent is valid
                        if (inSpawn.Parent == null)
                        {
                            Debug.Log("Null parent");
                            return null;
                        }

                        // Try to spawn from our parent
                        result = inSpawn.Parent.createSpawnableInstance();
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns true of the spawner is configured correctly.
        /// </summary>
        /// <param name="inSpawn">The spawn point to validate</param>
        /// <returns>True is the spawner is correctly configured</returns>
        public static bool isValidConfiguration(this ISpawn inSpawn)
        {
            // Check for correct setup
            if (inSpawn.SpawnSettings == SpawnSettings.Custom)
                if (inSpawn.Spawner == null)
                    return false;

            return true;
        }

        /// <summary>
        /// Maintains an enumerator state between calls so thet subsiquent calls to this method will return the next spawn inline.
        /// </summary>
        /// <param name="inSpawn">Extension input</param>
        /// <returns></returns>
        public static ISpawn sequentialSpawn(this ISpawn inSpawn)
        {
            // Check for existing
            if (sequential.ContainsKey(inSpawn) == false)
                sequential.Add(inSpawn, inSpawn.GetEnumerator());

            // Get the keypair
            IEnumerator<ISpawn> enumerator = sequential[inSpawn];

            // Try to move the enumerator otherwise reset it
            if (enumerator.MoveNext() == false)
            {
                // Reset the enumerator (Cant use Reset in this case), Move next makes usre we start at the first item
                sequential[inSpawn] = inSpawn.GetEnumerator();
                sequential[inSpawn].MoveNext();

                // Update the local enumerator
                enumerator = sequential[inSpawn];
            }

            // Get the current spawn
            return enumerator.Current;
        }

        /// <summary>
        /// Returns the size of a generic IEnumerable collection.
        /// Removes dependencies on Linq.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="collection">Extension input collection</param>
        /// <returns></returns>
        public static int size<T>(this IEnumerable<T> collection)
        {
            int count = 0;

            // Iterate through the collection incrementing the count
            foreach (T item in collection)
            {
                count++;
            }

            // Get the count
            return count;
        }

        /// <summary>
        /// Returns an element at the specified index if available otherwise the default value for the generic type.
        /// Removes dependencies on Linq.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="collection">Extension input collection</param>
        /// <param name="index">The index of the item</param>
        /// <returns></returns>
        public static T element<T>(this IEnumerable<T> collection, int index)
        {
            int count = 0;

            // Iterate through the collection until we find the correct index
            foreach (T item in collection)
            {
                // Check for matching index
                if (count == index)
                    return item;

                // Increment the count
                count++;
            }

            // Not found or index out of bounds
            return default(T);
        }
    }
}
