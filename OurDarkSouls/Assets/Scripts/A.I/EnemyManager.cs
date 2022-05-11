using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SG
{
    public class EnemyManager : CharacterManager
    {
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        


        public State currentState;   //Новое
        public CharacterStatsManager currentTarget; //Новое
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public bool isPreformingAction; 
        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;

        [Header ("A.I. Settings")]
        public float detectionRadius = 200;        
        public float maximumDetectionAngle = 50;
        public float mininumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        [Header("A.I Cobat Settings")]
        public bool allowAIPerfomConbos;
        public float comboLikelyHood;


        
        


        private void Awake() 
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start() 
        {
            enemyRigidbody.isKinematic = false;    
        }

        private void Update() 
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
        }

        private void LateUpdate()
        {            
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        { 
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);
            
                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }
            
        private void SwitchToNextState(State state)
        {
            currentState = state;
        }
        
        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {                
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPreformingAction)
            {
               if (currentRecoveryTime <= 0)
               {
                   isPreformingAction = false;
               }
            }
        }
    }
}

