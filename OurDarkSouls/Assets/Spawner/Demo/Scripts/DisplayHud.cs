using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UltimateSpawner.Demo
{
    /// <exclude/>
    public class DisplayHud : MonoBehaviour
    {
        // Private
        private static DisplayHud instance = null;
        private bool isRunning = false;

        // Public
        public WaveController waveManager;
        public Text currentText;
        public Text remainingText;
        public Text toSpawnText;
        public Text newWaveText;

        // Methods	
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            // Disabel wave text to start
            setAlpha(0);

            if (waveManager != null)
            {
                // Subscribe to event
                waveManager.onEnterWave += onEnterNewWave;
            }
        }

        private void Update()
        {
            try
            {
                currentText.text = string.Format("Current Wave: {0}", waveManager.CurrentWave);
                remainingText.text = string.Format("Enemies Remaining: {0}", waveManager.InstancesRemaining);
                toSpawnText.text = string.Format("Enemies to Spawn: {0}", waveManager.InstancesToSpawn);
            }
            catch { }
        }

        public static void showFadeText(string message)
        {
            // CHeck for a valid instance
            if (instance != null)
            {
                // Run the fade routine
                instance.StartCoroutine(instance.showFadeTextRoutine(message));
            }
        }

        private void onEnterNewWave(Wave wave)
        {
            // Display a message
            showFadeText("Next Wave!");
        }

        private IEnumerator showFadeTextRoutine(string text)
        {
            // Only allow one message at a time
            if (isRunning == true)
                yield break;

            // Set the running flag
            isRunning = true;

            // Enable the text
            newWaveText.text = text;
            setAlpha(1);

            // Fade value
            float fade = 1;

            // Wait for atleast 1 second
            yield return new WaitForSeconds(1);

            // Run fade loop
            while (fade > 0)
            {
                // Subtract from fade
                fade -= 0.01f;

                // Update the alpha
                setAlpha(fade);

                // Wait for next frame
                yield return null;
            }

            isRunning = false;
        }

        private void setAlpha(float val)
        {
            // Store the colour
            Color temp = newWaveText.color;

            // Change the alpha only
            newWaveText.color = new Color(temp.r, temp.g, temp.b, val);
        }
    }
}
