using UnityEngine;
using System.Collections;

#if ULTIMATE_SPAWNER_NETWORKED == true
using UnityEngine.Networking;
#endif

namespace UltimateSpawner.Demo
{
    /// <exclude/>
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
#if ULTIMATE_SPAWNER_NETWORKED == true
    public class TargetAI : NetworkBehaviour
#else    
    public class TargetAI : MonoBehaviour
#endif
    {
        // Private
        private UnityEngine.AI.NavMeshAgent agent = null;
        private Transform target = null;

        // Public

        // Properties

        // Methods
        private void Start()
        {
#if ULTIMATE_SPAWNER_NETWORKED == true
            if(isServer == false)
            {
                enabled = false;
                return;
            }
#endif

            // Get the agent component
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Get the player position
            updateTarget();

            // Delayed retarget
            InvokeRepeating("updateTarget", 1.5f, 1.5f);
        }

        private void Update()
        {
            // Update the ai's target
            agent.SetDestination(target.position);
        }

        private void updateTarget()
        {
            GameObject[] valid = GameObject.FindGameObjectsWithTag("Player");

            // Check for empty
            if (valid.Length == 0)
            {
                target = null;
                return;
            }

            // Select the closest
            float smallest = float.MaxValue;

            foreach (GameObject temp in valid)
            {
                // Find distance
                float distance = Vector3.Distance(temp.transform.position, transform.position);

                // Check if we have found a closer target
                if (distance < smallest)
                {
                    target = temp.transform;
                    smallest = distance;
                }
            }
        }
    }
}
