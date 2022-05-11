using UnityEngine;
using System.Collections;
 namespace SG 
 {
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        public GameObject[] trailParticles;
        public Vector3 impactNormal; //Used to rotate impactparticle.
    
        private bool hasCollided = false;
        CharacterStatsManager spellTarget;

        void Start()
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation);
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation);
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (!hasCollided)
            {
                spellTarget = other.transform.GetComponent<CharacterStatsManager>();
                
                if(spellTarget != null)
                    {
                        spellTarget.TakeDamage(currentWeaponDamage);
                    }
                hasCollided = true;
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticle);
                Destroy(impactParticle, 5f);
                Destroy(gameObject, 5f);



            }
        }
    }
}