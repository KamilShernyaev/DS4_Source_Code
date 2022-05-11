using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsorption;

        private void Awake() 
        {
            blockingCollider = GetComponent<BoxCollider>();    
        }

        public void SetColliderDamageAbsorption(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
            }
        }

        public void EnableBlockCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockCollider()
        {            
            blockingCollider.enabled = false;
        }
    }
}