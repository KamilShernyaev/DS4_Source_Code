using UnityEngine;
using System.Collections;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner
{
    /// <summary>
    /// The main class responsible for creating and maintaining waves.
    /// </summary>    
    public class WaveController : Controller
    {
        // Enum
        /// <summary>
        /// What action to take when the wave controller has finished its process.
        /// </summary>
        public enum WavesCompleteAction
        {
            /// <summary>
            /// Stop spawning.
            /// </summary>
            DoNothing = 0,
            /// <summary>
            /// Repeat all waves starting from the begining.
            /// </summary>
            RepeatFromStart,
            /// <summary>
            /// Repeat the last wave.
            /// </summary>
            RepeatLast,
        }

        /// <summary>
        /// The mode that the wave controller uses for wave advance.
        /// </summary>
        public enum WaveControllerMode
        {
            /// <summary>
            /// The wave controller should use the assigned waves.
            /// </summary>
            Preset = 0,
            /// <summary>
            /// The wave controller should generate an infinite number of waves
            /// </summary>
            Continuous,
            /// <summary>
            /// The wave controller should use <see cref="Preset"/> waves at the start and then transition to <see cref="Continuous"/> waves once complete.  
            /// </summary>
            Mixed,
        }

        // Delegates
        /// <summary>
        /// Delegate used for wave events.
        /// </summary>
        /// <param name="wave"></param>
        public delegate void WaveDelegate(Wave wave);

        /// <summary>
        /// Delegate used for progress events.
        /// </summary>
        /// <param name="progress"></param>
        public delegate void WaveProgressDelegate(float progress);

        // Events
        /// <summary>
        /// Triggered when the current wave has started
        /// </summary>
        public event WaveDelegate onEnterWave;

        /// <summary>
        /// Triggered when a boss wave starts.
        /// </summary>
        public event WaveDelegate onEnterBossWave;

        /// <summary>
        /// Triggered when the current wave has been completed
        /// </summary>
        public event WaveDelegate onExitWave;

        /// <summary>
        /// Triggered when a boss wave ends.
        /// </summary>
        public event WaveDelegate onExitBossWave;

        /// <summary>
        /// Triggered when continuous waves are disabled and the final wave has been completed
        /// </summary>
        public event WaveDelegate onExitLastWave;

        /// <summary>
        /// Triggered when the wave advances and allows values such as enemy health to be increased according to the current wave.
        /// </summary>
        public event WaveProgressDelegate onWaveProgress;

        // Private
        private Wave activeWave = null;
        private int currentWave = 0;
        private int currentIndex = 0;
        private int spawnedCount = 0;
        private int bossCount = 0;

        // Public
        /// <summary>
        /// Should the controller start automatically.
        /// </summary>
        [Tooltip("Should the waves start automatically")]
        public bool playOnStart = true;

        /// <summary>
        /// The maximum number of items that can exist at any time.
        /// </summary>
        [Tooltip("The maximum number of spawnable items that can exist at any time")]
        public int maximumSpawnedItems = 12;

        /// <summary>
        /// What action to take once the controller completes its cycle.
        /// </summary>
        [Tooltip("What to do when all waves have finished")]
        public WavesCompleteAction onComplete = WavesCompleteAction.DoNothing;

        /// <summary>
        /// Should the controller automatically generate an infinite amount of waves of progressive difficulty.
        /// </summary>
        [Tooltip("The advance mode used to generate the next wave. Continuous - The next wave is automatically generated based on the advace rates. Preset - The next wave is selected from the preset wave array. Mixed - Preset modes will be used initially and will then switch to continuous mode to provide an infinite number of waves")]
        public WaveControllerMode waveMode = WaveControllerMode.Continuous;

        /// <summary>
        /// The advance rate at which the spawn amount is increased pre wave.
        /// </summary>
        [Tooltip("The rate at which wave difficult is increased when using continuous waves. The higher the number, the quicker the spawn count rises")]
        [Range(1.0f, 10.0f)]
        public float spawnAdvanceRate = 1.5f;

        /// <summary>
        /// The advance rate at which wave delays are reduced per wave. Allows for less cool-off between waves to increase difficulty.
        /// </summary>
        [Tooltip("The rate at which wave start, end and spawn delay timings will advance to increase difficulty. The higher the number, the quicker the difficulty rises")]
        [Range(1.0f, 10.0f)]
        public float delayAdvanceRate = 1.1f;
                
        /// <summary>
        /// The start wave used to generate all other waves. Only used when continuous waves is enabled.
        /// </summary>
        [Tooltip("The information for the starting wave, All generated waves will use this wave as a template to advance")]
        public Wave startWave;
        
        /// <summary>
        /// An array of user defined waves that allow more control over progression. Only used when continuous waves is disabled.
        /// </summary>
        [Tooltip("An array of user defined waves that will execute in order")]
        public Wave[] userWaves;

        /// <summary>
        /// Should the contorller include boss waves.
        /// </summary>
        [Tooltip("Should the controller use boss waves")]
        public bool useBossWaves = false;

        /// <summary>
        /// Should the boss wave be treated as a normal wave. If so, the wave counter will increase and wave start and end events will trigger.
        /// </summary>
        [Tooltip("Should the boss round be treated like another wave. If not, the wave counter will not increate until the boss round is finished")]
        public bool bossRoundIsWave = false;

        /// <summary>
        /// How many waves should pass before a boss wave is triggered. 
        /// </summary>
        [Tooltip("The amount of waves to wait before the boss round activates")]
        public int bossWaveRound = 3;

        /// <summary>
        /// The information for the boss wave.
        /// </summary>
        [Tooltip("The information for boss waves")]
        public Wave bossWave;
                

        // Properties
        /// <summary>
        /// Get a reference to the current wave
        /// </summary>
        public int CurrentWave
        {
            get { return currentWave; }
        }

        /// <summary>
        /// How many items are left to spawn.
        /// </summary>
        public int InstancesToSpawn
        {
            get
            {
                int result = activeWave.spawnCount - spawnedCount;

                if (result < 0)
                    result = 0;

                return result;
            }
        }

        /// <summary>
        /// How many items are left alive.
        /// </summary>
        public int InstancesRemaining
        {
            get { return spawnManager.Spawner.InstancesRemaining; }
        }

        // Methods
        private void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if(isServer == false)
                NetError.raise();
#endif

            if (spawnManager == null)
            {
                Debug.LogError("Wave manager has not been setup correctly. Make sure that the spawn manager field is assigned");
                enabled = false;
                return;
            }

            // Launch wave immediatley
            if (playOnStart == true)
                startSpawning();
        }

        /// <summary>
        /// Begin spawning enemies using the wave settings (Called automatically if playOnStart is true)
        /// </summary>
        public override void startSpawning()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if(isServer == false)
                NetError.raise();
#endif

            // Run the coroutine
            StartCoroutine(waveRountine());
        }

        /// <summary>
        /// Stop spawning enemies immediatley (The current wave will be abandoned and the next call to startWave will advance to the next wave)
        /// </summary>
        public override void stopSpawning()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Stop spawning
            StopCoroutine(waveRountine());
        }

        /// <summary>
        /// Resets the wave manager to the first round without starting spwning.
        /// </summary>
        public void reset(bool keepState = false)
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if (isServer == false)
                NetError.raise();
#endif

            // Reset values
            activeWave = null;            
            currentIndex = 0;

            // Should we keep the wave state
            if (keepState == false)
            {
                currentWave = 0;
                spawnedCount = 0;
            }
        }

        /// <summary>
        /// Stops the current wave from spawning any more enemies resets the manager to the starting round and restarts spawning.
        /// Essentially starting from the first round again.
        /// </summary>
        public void restart()
        {
            stopSpawning();
            reset();
            startSpawning();
        }

        private IEnumerator waveRountine()
        {
            bool bossWave = false;
            Wave lastWave = activeWave;

            // Get the wave
            Wave wave = getNextWave(out bossWave);

            // Make sure we have a valid wave
            if (wave == null)
                yield break;
            
            activeWave = wave;

            if (bossWave == false)
            {
                // Trigger wave started
                onWaveStarted();
            }
            else
            {
                // Trigger the wave event
                if (bossRoundIsWave == true)
                    onWaveStarted();

                // Trigger the boss wave start
                onBossWaveStarted();
            }

            // Wait for the start delay
            yield return new WaitForSeconds(activeWave.waveStartDelay);

            // Begin spawning
            for(int i = 0; i < activeWave.spawnCount; i++)
            {
                // Make sure there is room for more spawnables
                while (InstancesRemaining >= maximumSpawnedItems)
                    yield return null;                

                // Spawn flag
                bool canSpawn = false;

                // Get the spawn manager
                SpawnBase spawn = (activeWave.spawnManager != null) ? activeWave.spawnManager : spawnManager;

                // Wait until we can spawn
                while (canSpawn == false)
                {
                    // Check if we are able to spawn
                    canSpawn = spawn.canSpawn();
                    yield return null;
                }

                // Spawn the enemy
                spawn.spawn();
                spawnedCount++;

                // Wait for next spawn
                yield return new WaitForSeconds(activeWave.randomSpawnDelay());
            }

            // Wait for all enemies to become dead
            if (activeWave.waitForDead == true)
                while (InstancesRemaining > 0)
                    yield return null;

            // Wait for end of wave delay
            yield return new WaitForSeconds(activeWave.waveCompleteDelay);

            if (bossWave == false)
            {
                // Trigger wave end
                onWaveEnded();
            }
            else
            {
                // Trigger boss wave end
                onBossWaveEnded();

                // Trigger wave end event
                if (bossRoundIsWave == true)
                    if (onExitWave != null)
                        onExitWave(activeWave);

                // Revert to the previous wave so that wave advance works correctly
                activeWave = lastWave;
            }
        }
        
        private Wave getNextWave(out bool isBoss)
        {
            // Check for boss waves
            if(useBossWaves == true)
            {
                // Check if we can activate the boss round
                if(bossCount >= bossWaveRound)
                {
                    // We can start the boss round
                    bossCount = 0;

                    // Check if the boss round is another wave
                    if(bossRoundIsWave == true)
                    {
                        // Update values
                        currentWave++;
                        currentIndex++;
                        spawnedCount = 0;
                    }

                    isBoss = true;
                    return bossWave;
                }

                bossCount++;
            }

            // Update values
            currentWave++;
            currentIndex++;
            spawnedCount = 0;

            // Check for continuous waves
            switch(waveMode)
            {
                // Continuous wave mode
                case WaveControllerMode.Continuous:
                    {
                        // Get the next wave
                        return getNextContinuousWave(out isBoss);
                    } // End continuous wave mode

                    // Preset wave mode
                case WaveControllerMode.Preset:
                    {
                        return getNextPresetWave(out isBoss);
                    } // End preset wave mode

                case WaveControllerMode.Mixed:
                    {
                        // Check if we have completed the fixed wave stage
                        if((currentIndex - 1) < userWaves.Length)
                        {
                            // Use a preset wave
                            return getNextPresetWave(out isBoss);
                        }
                        else
                        {
                            // Use a continuous wave
                            return getNextContinuousWave(out isBoss);
                        }
                    }
            }

            // Something went wrong
            isBoss = false;
            return null;
        }

        private Wave getNextContinuousWave(out bool isBoss)
        {
            // Check for first round
            if (activeWave == null)
            {
                isBoss = false;
                return startWave;
            }

            // Get the generated wave
            Wave result = activeWave.generateNextWave(spawnAdvanceRate, delayAdvanceRate);

            // Trigger event
            if (onWaveProgress != null)
                onWaveProgress(spawnAdvanceRate);

            // Get the value
            isBoss = false;
            return result;
        }

        private Wave getNextPresetWave(out bool isBoss)
        {
            // Try to get the first wave
            if (activeWave == null)
            {
                if (userWaves != null && userWaves.Length > 0)
                {
                    isBoss = false;
                    return userWaves[0];
                }
            }

            // Make sure the wave is valid
            if (currentIndex - 1 < userWaves.Length)
            {
                // Get the next wave                    
                Wave result = userWaves[currentIndex - 1];

                // Trigger event
                if (onWaveProgress != null)
                    onWaveProgress(activeWave.calculateAdvanceRate(result));

                isBoss = false;
                return result;
            }

            // Looks like we have completed our wave list
            switch (onComplete)
            {
                case WavesCompleteAction.DoNothing:
                    {
                        // Revert to last
                        currentIndex--;
                        currentWave--;

                        // Trigger last wave
                        if (onExitLastWave != null)
                            onExitLastWave(activeWave);
                    }
                    break;

                case WavesCompleteAction.RepeatFromStart:
                    {
                        // Reset
                        reset(true);

                        // Reccursive call
                        isBoss = false;
                        return getNextWave(out isBoss);
                    }

                case WavesCompleteAction.RepeatLast:
                    {
                        // Get the last wave
                        isBoss = false;
                        return userWaves[userWaves.Length - 1];
                    }
            }

            isBoss = false;
            return null;
        }

        #region Events
        private void onWaveStarted()
        {
            // Trigger event
            if (onEnterWave != null)
                onEnterWave(activeWave);
        }

        private void onWaveEnded()
        {
            // Trigger event
            if (onExitWave != null)
                onExitWave(activeWave);

            // Trigger the next wave
            startSpawning();
        }

        private void onBossWaveStarted()
        {
            // Trigger event
            if (onEnterBossWave != null)
                onEnterBossWave(activeWave);
        }

        private void onBossWaveEnded()
        {
            // Trigger event
            if (onExitBossWave != null)
                onExitBossWave(activeWave);

            // Trigger the next wave
            startSpawning();
        }
        #endregion
    }
}
