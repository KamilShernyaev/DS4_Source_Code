using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    
    public class PlayerManager : CharacterManager
    {
        public InputHandler inputHandler;
        
        public Animator animator;
        public UIManager uIManager;
        public CameraHandler cameraHandler;
        public PlayerStatsManager playerStatsManager;
        public PlayerInventoryManager playerInventoryManager;        
        public PlayerEquipmentManager playerEquipmentManager;
        public PlayerLocomotionManager playerLocomotionManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;

        public CriticalDamageCollider criticalDamageCollider;
        public PlayerAnimatorManager playerAnimatorManager;

        public InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        private void Awake() 
        {
          uIManager = FindObjectOfType<UIManager>();
          playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
          playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
          inputHandler = GetComponent<InputHandler>();
          playerInventoryManager = GetComponent<PlayerInventoryManager>();
          animator = GetComponent<Animator>();
          playerStatsManager = GetComponent<PlayerStatsManager>();
          playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
          interactableUI = FindObjectOfType<InteractableUI>();
          cameraHandler = FindObjectOfType<CameraHandler>();
          criticalDamageCollider = GetComponentInChildren<CriticalDamageCollider>();
          playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        void Update() 
        {
            float delta = Time.deltaTime;
            
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");            
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");   
            animator.SetBool("isBlocking",isBlocking);
            animator.SetBool("isInAir", isInAir);    
            animator.SetBool("isDead", playerStatsManager.isDead);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");

            inputHandler.TickInput(delta);
            playerLocomotionManager.HandleRollingAndSprinting(delta);
            playerLocomotionManager.HandleJumping();
            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();
        }

         private void FixedUpdate() 
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
            playerLocomotionManager.HandleMovement(delta);
            playerLocomotionManager.HandleRotation(delta);
        }

        private void LateUpdate() 
        {            
            inputHandler.rollFlag = false;
            inputHandler.rt_input = false;
            inputHandler.rb_input = false;   
            inputHandler.lt_input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false; 
            inputHandler.jump_Input = false;   
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;

            if(cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotionManager.isAirTimer = playerLocomotionManager.isAirTimer + Time.deltaTime;
            }
        }

        #region Player Interactions

       

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
        
        public void OpenChestInteraction(Transform playerStandHereWhenOpeningChest)
        {
            playerLocomotionManager.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandHereWhenOpeningChest.transform.position;
            Debug.Log("PlayerManager");
           
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        #endregion
    }
}