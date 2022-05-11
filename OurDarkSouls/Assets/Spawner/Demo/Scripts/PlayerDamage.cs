using UnityEngine;
using System.Collections;

namespace UltimateSpawner.Demo
{
    /// <exclude/>
    public class PlayerDamage : MonoBehaviour
    {
        // Methods
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Get the highest level transform
            Transform hitRoot = hit.transform.root;

            // Try to get the target ai script
            TargetAI ai = hitRoot.GetComponent<TargetAI>();

            // Check for component
            if (ai != null)
            {
                // Display a message on screeen
                DisplayHud.showFadeText("You Died!");
                PlayerSpawner spawner = Component.FindObjectOfType<PlayerSpawner>();

                // Makes sure we found the spawner
                if (spawner != null)
                {
                    // Reposition the player at a spawn point
                    spawner.respawnPlayer();
                }
            }
        }
    }
}
