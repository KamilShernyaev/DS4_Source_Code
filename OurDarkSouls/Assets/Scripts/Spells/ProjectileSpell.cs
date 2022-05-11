using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;
        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;
        Rigidbody rigidBody;

        public override void AttemptToCastSpell(
            PlayerAnimatorManager playerAnimatorManager, 
            PlayerStatsManager playerStatsManager, 
            PlayerWeaponSlotManager playerWeaponSlotManager)
        {
            base.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
            instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
            playerAnimatorManager.PlayTargetAnimation(spellAnimation,true);
            Destroy(instantiatedWarmUpSpellFX, 1f);
        }

        public override void SuccessfullyCastSpell(
            PlayerAnimatorManager playerAnimatorManager,
            PlayerStatsManager playerStatsManager, 
            CameraHandler cameraHandler, 
            PlayerWeaponSlotManager playerWeaponSlotManager)
        {
            base.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
            GameObject instantiedSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidBody = instantiedSpellFX.GetComponent<Rigidbody>();
            // spellDamageCollider = instantiedSpellFX.GetComponent<SpellDamageCollider>();

            if(cameraHandler.currentLockOnTarget != null)
            {
                instantiedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
            }
            rigidBody.AddForce(instantiedSpellFX.transform.forward * projectileForwardVelocity);
            rigidBody.AddForce(instantiedSpellFX.transform.up * projectileUpwardVelocity);
            rigidBody.useGravity = isEffectedByGravity;
            rigidBody.mass = projectileMass;
            instantiedSpellFX.transform.parent = null;
        }
    }
}
