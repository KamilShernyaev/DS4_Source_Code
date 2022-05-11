using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace UltimateSpawner
{
    /// <summary>
    /// Represents a game round as a wave using the settigns specified.
    /// </summary>
    [Serializable]
    public class Wave
    {
        // Private
        private const float waveTimeFactor = 0.2f;

        // Public
        /// <summary>
        /// The spawner associated with ths wave. This field may be left blank in which case the wave controller's spawner is used.
        /// </summary>
        [Tooltip("The spawnable manager to use for this wave. Overrides the wave controllers spawnable manager when used. This field can be left blank and the wave controllers spawner will be used.")]
        public SpawnBase spawnManager;

        /// <summary>
        /// When true, the spawner will wait for all enemies of the current wave to be killed before proceeding to the next wave.
        /// </summary>
        [Tooltip("Wait for all enemies to die before starting the next wave")]
        public bool waitForDead = true;

        /// <summary>
        /// The minimum amount of time to wait before spawning another enemy.
        /// </summary>
        [Tooltip("The minimum time (In seconds) to wait before spawning another enemy")]
        public float spawnFrequency = 10;

        /// <summary>
        /// The amount of random time to include in the spawn frequency calculations.
        /// </summary>
        [Tooltip("The amount of randomness to include in the wait time (In seconds)")]
        public float spawnRandomness = 5;

        /// <summary>
        /// The amount of time to wait before the next wave can start.
        /// </summary>
        [Tooltip("The amount of time to wait before starting this wave")]
        public float waveStartDelay = 5;

        /// <summary>
        /// The amount of time to wait before the current wave can end.
        /// </summary>
        [Tooltip("The amount of time to wait before starting the next wave")]
        public float waveCompleteDelay = 12;

        /// <summary>
        /// The amount of enemies to spawn for this wave
        /// </summary>
        [Tooltip("The number of enemies to spawn for this wave")]
        public int spawnCount = 8;

        // Methods
        /// <summary>
        /// Calculates the time when the next enemy should be spawned.
        /// </summary>
        /// <returns>A delay time in seconds</returns>
        public float randomSpawnDelay()
        {
            // Get the frequency
            float delay = spawnFrequency;

            // Calcualte the randomness margin
            float randomness = Random.Range(0, spawnRandomness);

            // Add to the delay
            return delay + randomness;
        }

        /// <summary>
        /// Responsible for creating an instance of the wave class that represents the next wave in sequece from this current wave.
        /// The new values for the wave should be generated based on the progress rates supplied so that advancment works as expected.
        /// </summary>
        /// <param name="spawnRate">How much to progress the wave spawn count by</param>
        /// <param name="delayRate">How much to progress the wave timings by</param>
        /// <returns>A reference to an instance of the Wave class that represents an advanced wave</returns>
        public Wave generateNextWave(float spawnRate, float delayRate)
        {
            Wave next = new Wave();

            // Straight copy
            next.waitForDead = waitForDead;

            float delayFactor = 1 / delayRate;

            // Progress rate modifier
            next.spawnFrequency = spawnFrequency * delayFactor;
            next.spawnRandomness = spawnRandomness * delayFactor;
            next.waveStartDelay = waveStartDelay * delayFactor;
            next.waveCompleteDelay = waveCompleteDelay * delayFactor;

            // Spawn rate modifier
            next.spawnCount = (int)(spawnCount * spawnRate);

            return next;
        }

        /// <summary>
        /// Calculates the differences between waves to find an advance value.
        /// </summary>
        /// <param name="next">The next wave in the sequence</param>
        /// <returns>A values greater than 1 representing the progression rate</returns>
        public float calculateAdvanceRate(Wave next)
        {
            // Get the progress rate as a decimal
            float rate = (1 / spawnCount) * next.spawnCount;

            return rate;
        }
    }
}
