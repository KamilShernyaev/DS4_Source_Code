using UnityEngine;
using System.Collections;

namespace UltimateSpawner.Demo
{
    public sealed class TopDownControl : MonoBehaviour
    {
        private enum Direction
        {
            Forward = 0,
            Right,
            Backward,
            Left,
        }

        // Private
        private const float offset = -90;        
        private bool isMoving = true;

        // Public
        public float speed = 2;

        // Methods
        public void Update()
        {
            // Reset the moving flag
            isMoving = false;

            // Left
            if(Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true)
            {
                changeDirection(Direction.Left);
                isMoving = true;
            }

            // Right
            if(Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true)
            {
                changeDirection(Direction.Right);
                isMoving = true;
            }

            // Up
            if(Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true)
            {
                changeDirection(Direction.Backward);
                isMoving = true;
            }

            // Down
            if(Input.GetKey(KeyCode.DownArrow) == true ||Input.GetKey(KeyCode.S) == true)
            {
                changeDirection(Direction.Forward);
                isMoving = true;
            }


            // CHeck for moving
            if(isMoving == true)
            {
                // Move the player
                transform.position += transform.right * (speed * Time.deltaTime);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            // We have collider with the player
            if (collision.gameObject.GetComponent<TopDownEnemy>() != null)
            {
                // Let the spawn manager know that this enemy has dies
                SpawnableManager.informSpawnableDestroyed(collision.gameObject, true);
            }
        }

        private void changeDirection(Direction direction)
        {
            // Rotate the sprite
            transform.eulerAngles = new Vector3(0, 0, ((float)direction * 90) + offset);
        }
    }
}
