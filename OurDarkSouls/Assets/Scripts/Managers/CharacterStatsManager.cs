using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        public int maxHelth;
        public int currentHealth;

        
        public float maxStamina;
        public float currentStamina;

       public bool isDead;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int currentSoulCount = 0;
        public int soulsAwardedOnDeath = 50;

        
        [Header("Character Level")]
        public int playerLevel = 1;

        [Header("Levels")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strengthLevel = 10;
        public int dexeterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;


        [Header("Armor Absorptions")]
        public float physicalDamageAbsoptionHead;
        public float physicalDamageAbsoptionBody;
        public float physicalDamageAbsoptionLegs;
        public float physicalDamageAbsoptionHand;

        private void Awake() 
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "TakeDamage")
        {
                
                float totalPhysicalDamageAbsorption = 1 - 
                (1 - physicalDamageAbsoptionHead / 100) * 
                (1 - physicalDamageAbsoptionBody / 100) * 
                (1 - physicalDamageAbsoptionLegs / 100) * 
                (1 - physicalDamageAbsoptionHand / 100);

                physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

                Debug.Log("Total Damage Absorption is" + totalPhysicalDamageAbsorption + "%");

                float finalDamage = physicalDamage;
                currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

                Debug.Log("Total Damage Deal is" + finalDamage);

                if(currentHealth <= 0)
                {
                    currentHealth = 0;
                    isDead = true;
                }
        }
    
        public virtual void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;
            
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        
        public int SetMaxHealthFromHealthLevel()
        {
            maxHelth = healthLevel * 10;
            return maxHelth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }
    }
}
