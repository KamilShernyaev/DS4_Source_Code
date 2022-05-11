using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public bool enabledDamageColliderOnStartUp = false;
        public int currentWeaponDamage = 25;

        private void Awake() 
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartUp;


        }

        public void EnableDamageColider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        
        private void OnTriggerEnter(Collider collision) 
        {
            if(collision.tag == "Player")
            {
                PlayerStatsManager playerStatsManager = collision.GetComponent<PlayerStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if(enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }
                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = 
                    currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                    if (playerStatsManager != null)
                    {
                        playerStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                        return;
                    }
                }

                if(playerStatsManager != null)
                {
                    playerStatsManager.TakeDamage(currentWeaponDamage);
                }
            }

            if(collision.tag == "Enemy")
            {
                EnemyStatsManager enemyStatsManager = collision.GetComponent<EnemyStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                // if (enemyCharacterManager != null)
                // {
                //     if(enemyCharacterManager.isParrying)
                //     {
                //         characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                //         Debug.Log("Тут");
                //         return;
                //     } 
                //     else if (shield != null && enemyCharacterManager.isBlocking)
                //     {
                //         float physicalDamageAfterBlock = 
                //         currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                //         if (enemyStatsManager != null)
                //         {
                //             enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                //             return;
                //         }
                //     }
                // }               

                if(enemyStatsManager != null)
                {
                    enemyStatsManager.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}