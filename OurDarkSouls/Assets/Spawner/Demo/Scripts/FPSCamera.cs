using UnityEngine;
using System.Collections;

namespace UltimateSpawner.Demo
{
    /// <exclude/>
    [RequireComponent(typeof(Camera))]
    public class FPSCamera : MonoBehaviour
    {
        // Private
        private Vector2 smooth = Vector2.zero;
        private Vector2 absolute = Vector2.zero;
        private bool lockToggle = true;

        // Public
        public Transform root = null;

        public float sensitivity = 2;
        public float aimSensitivity = 0.6f;
        public float smoothing = 3;
        public float lookAngle = 120;

        // Methods
        public void activateCamera()
        {
            // Enable the local camera
            GetComponent<Camera>().enabled = true;
        }

        public void deactivateCamera()
        {
            // Disable the local camera
            GetComponent<Camera>().enabled = false;
        }

        public void enableLocal()
        {
            // Allow the camera to become the main camera
            activateCamera();
        }

        public void disableLocal()
        {
            // Disable the camera
            deactivateCamera();
        }

        void Start()
        {
            // Update the initial rotation
            if (root != null)
            {
                // Store the initial rotation to prevent snap to 0
                absolute.x = root.rotation.eulerAngles.y;
            }
            else
            {
                // Store the initial rotation to prevent snap to 0
                absolute.x = transform.root.eulerAngles.y;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) == true)
                lockToggle = !lockToggle;
                        
            // Lock the sursor to the screen
            Cursor.lockState = (lockToggle == true) ? CursorLockMode.Locked : CursorLockMode.None;

            // Get mouse delta
            Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Get the modified input
            float modified = (sensitivity * smoothing);

            // Scale input base on sensitivity
            delta.Scale(new Vector2(modified, modified));

            // Interpolat movement
            smooth.x = Mathf.Lerp(smooth.x, delta.x, 1 / smoothing);
            smooth.y = Mathf.Lerp(smooth.y, delta.y, 1 / smoothing);

            // Find the absolute position
            absolute += smooth;

            // Clamp the angle
            absolute.y = Mathf.Clamp(absolute.y, -(lookAngle / 2), (lookAngle / 2));

            // Find the rotation around x
            Quaternion lookRotation = Quaternion.AngleAxis(-absolute.y, Vector3.right);
            transform.localRotation = lookRotation;

            // Check for root
            if (root != null)
            {
                // FInd the rotation around y
                Quaternion rotation = Quaternion.AngleAxis(absolute.x, root.up);
                root.localRotation = rotation;
            }
            else
            {
                Quaternion rotation = Quaternion.AngleAxis(absolute.x, transform.up);
                transform.localRotation = rotation;
            }
        }
    }
}
