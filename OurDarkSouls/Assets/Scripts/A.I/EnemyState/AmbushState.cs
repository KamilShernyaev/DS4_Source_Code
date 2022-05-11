using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AmbushState : State
    {
		public bool isSleeping;
		public float detectionRadius = 2;
		public string sleepAnimation;
		public string wakeAnimation;
		public LayerMask detectionLayer;

		public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
			if(isSleeping && enemyManager.isInteracting == false)
			{
				enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
			}

			#region  Handle Target Detection:

			Collider[] colliders = Physics.OverlapSphere(transform.transform.position, enemyManager.detectionRadius, detectionLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

				if(characterStats != null)
				{
					Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
					float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

					if(viewableAngle > enemyManager.mininumDetectionAngle &&
					viewableAngle < enemyManager.maximumDetectionAngle)
					{
						enemyManager.currentTarget = characterStats;
						isSleeping = false;
						enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
					}
				}
			}

			#endregion
	
			# region Handle State Change

			if (enemyManager.currentTarget != null)
			{
				return pursueTargetState;
			}
			else
			{
				return this;
			}

			# endregion
		}
    }
}