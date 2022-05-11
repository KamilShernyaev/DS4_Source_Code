using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        protected override void Awake() 
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
            animator = GetComponent<Animator>();
        }

        public void AwardSoulsOnDeath()
        {
            Debug.Log("Трутуту");
            PlayerStatsManager playerStatsManager = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if(playerStatsManager != null)
            {
                playerStatsManager.AddSouls(characterStatsManager.soulsAwardedOnDeath);
                
                if(soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStatsManager.currentSoulCount);
                }
            }
        }

        private void OnAnimatorMove() 
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}