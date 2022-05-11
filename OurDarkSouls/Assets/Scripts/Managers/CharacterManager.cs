using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterStatsManager characterStatsManager;

       [Header("Lock On Transform")]
       public Transform lockOnTransform;

       [Header("Combat Colliders")]
       public CriticalDamageCollider backStabCollider;
       public CriticalDamageCollider riposteCollider;
       
       [Header("Interaction")]
       public bool isInteracting;

       [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool canDoCombo;
        public bool isParrying;
        public bool isBlocking;
        public int pendingCriticalDamage;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;

        [Header("Movement Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isInvulnerable;
       
       private void Awake() 
       {
           characterStatsManager = GetComponent<CharacterStatsManager>();
       }
    }
}
