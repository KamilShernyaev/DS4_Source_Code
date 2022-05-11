using UnityEngine;
using System.Collections;

namespace UltimateSpawner.Demo
{
    public sealed class TopDownEnemy : MonoBehaviour
    {
        // Private
        private Transform target = null;

        // Public
        public float speed = 2;

        // Methods
        public void Start()
        {
            // Look for the player controller script
            TopDownControl control = Component.FindObjectOfType<TopDownControl>();

            // Set the target transform
            if (control != null)
                target = control.transform;
        }

        public void Update()
        {
            // Make sure we have a target
            if (target == null)
                return;

            // Look at target
            Vector3 direction = (target.position - transform.position);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Move towards
            transform.position += transform.right * (speed * Time.deltaTime);
        }        
    }
}
