using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        [Header("NAME")]
        public string playerName = "Nameless";
        CharacterManager character;
        PlayerManager playerManager;
        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;
        PlayerAnimatorManager playerAnimatorManager;
        public AudioSource takeDamageSound;

        public float staminaRegenerationAmount = 30;
        public float staminaRegenTimer = 0;
        [SerializeField] private CapsuleCollider playerCapsuleCollider;
        [SerializeField] private GameObject loseWindow;

        private void Awake() 
        {
            character = FindObjectOfType<CharacterManager>();
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            takeDamageSound = GetComponentInChildren<AudioSource>();
        }

        void Start()
        {   
            playerCapsuleCollider.enabled = true;

            maxHelth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHelth;
            healthBar.SetMaxHealth(maxHelth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }


        public override void TakeDamage(int damage, string damageAnimation = "TakeDamage")
        {
            if(playerManager.isInvulnerable)
                return;

            base.TakeDamage(damage, damageAnimation = "TakeDamage");
            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            takeDamageSound.Play();

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Death", true);
                playerCapsuleCollider.enabled = false;               
                loseWindow.SetActive(true); 
            }
        }       

        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }
    
        public void RegenerateStamina()
        {
            if(playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;
                if(currentStamina <= maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if(currentHealth > maxHelth)
            {
                currentHealth = maxHelth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }
    
        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }
    }
}
