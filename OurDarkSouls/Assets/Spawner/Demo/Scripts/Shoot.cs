using UnityEngine;
using System.Collections;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner.Demo
{
    /// <exclude/>
#if ULTIMATE_SPAWNER_NETWORKED == true
    public class Shoot : NetworkBehaviour
#else
    public class Shoot : MonoBehaviour
#endif
    {
        // Public
        public Camera playerCamera;

        // Methods
        private void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if(isLocalPlayer == false)
            {
                enabled = false;
                return;
            }
#endif
        }

        private void Update()
        {
            // Check for fire
            if (Input.GetButtonDown("Fire1") == true)
            {
                // Create a ray
                Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

                // Fire the ray into the scene
                RaycastHit[] hits = Physics.RaycastAll(ray, 100);

                foreach (RaycastHit hit in hits)
                {
                    // Dont process colliders
                    if (hit.collider.isTrigger == true)
                        continue;

                    takeHit(hit);
                }
            }
        }

        private void takeHit(RaycastHit hit)
        {
            // Get the game object
            GameObject hitObject = hit.collider.gameObject;

            // Find the highest level transform
            Transform hitRoot = hitObject.transform.root;

#if ULTIMATE_SPAWNER_NETWORKED == true
            NetworkIdentity id = hitRoot.GetComponent<NetworkIdentity>();

            // We have hit a static scene object
            if (id == null)
                return;

            // We have hit a non-enemy object (Another player)
            if (id.GetComponent<TargetAI>() == null)
                return;

            // Kill the enemy
            Cmd_takeHit((int)id.netId.Value);
#else
            // Try to get the ai script
            TargetAI ai = hitRoot.GetComponentInChildren<TargetAI>();


            // Check for the target AI component
            if (ai != null)
            {
                // Inform the enemy manager that an enemy has died
                SpawnableManager.informSpawnableDestroyed(hitRoot.gameObject, true);
            }
#endif
        }

#if ULTIMATE_SPAWNER_NETWORKED == true
        [Command]
        private void Cmd_takeHit(int objectID)
        {
            foreach (NetworkIdentity identity in Component.FindObjectsOfType<NetworkIdentity>())
            {
                if (identity.netId.Value == objectID)
                {
                    // Try to get the ai script
                    TargetAI ai = identity.GetComponentInChildren<TargetAI>();

                    // Check for the target AI component
                    if (ai != null)
                    {
                        // Inform the enemy manager that an enemy has died
                        SpawnableManager.informSpawnableDestroyed(identity.gameObject, true);
                    }

                    break;
                }
            }
        }
#endif
    }
}
