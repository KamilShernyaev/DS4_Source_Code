using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool a_Input;
        public bool b_Input;
        public bool x_Input;
        public bool y_Input;
        public bool rb_input;
        public bool rt_input;
        public bool lb_input;
        public bool lt_input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public Transform criticalAttackRayCastStartPoint;
        
        PlayesControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManagerManager;
        PlayerManager player;
        PlayerEffectsManager playerEffectsManager;
        PlayerStatsManager playerStatsManager;
        BlockingCollider blockingCollider;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        CameraHandler cameraManager;
        PlayerAnimatorManager playerAnimatorManager;
        UIManager uIManager;

        Vector2 movementInput;
        Vector2 cameraInput;       

        private void Awake() 
          {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManagerManager = GetComponent<PlayerInventoryManager>();
            player = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            uIManager = FindObjectOfType<UIManager>();
            cameraManager = FindObjectOfType<CameraHandler>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
          }

        public void OnEnable() 
          {
            if(inputActions == null)
              {
                inputActions = new PlayesControls();

                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.RB.performed += i => rb_input = true;
                inputActions.PlayerActions.RT.performed += i => rt_input = true;
                inputActions.PlayerActions.LT.performed += i => lt_input = true;
                inputActions.PlayerActions.LB.performed += i => lb_input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_input = false;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i  => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i  => right_Stick_Left_Input = true; 
                inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
              }
              inputActions.Enable();
          }
          private void OnDisable() 
          {
              inputActions.Disable();
          }
          public void TickInput(float delta) 
          {
            if (playerStatsManager.isDead)
              return;

              HandleMoveInput(delta);
              HandleRollInput(delta);
              HandleCombatInput(delta);
              HandleQuickSlotsInput();
              HandleInventoryInput();
              HandleLockOnInput();
              HandleTwoHandInput();
              HandleCritivalAttackInput();
              HandleUseConsumbleInput();
          }
          private void HandleMoveInput(float delta)

          {
              horizontal = movementInput.x;
              vertical = movementInput.y;
              moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
              mouseX = cameraInput.x;
              mouseY = cameraInput.y;
          }

          private void HandleRollInput(float delta)
          {
            if(b_Input)
            {
              rollInputTimer += delta;

              if(playerStatsManager.currentStamina <= 0)
              {
                b_Input = false;
                sprintFlag = false;
              }

              if(moveAmount >0.5f && playerStatsManager.currentStamina > 0)
              {
                sprintFlag = true;
              }
            }
            else
            {
              sprintFlag = false;

              if(rollInputTimer > 0 && rollInputTimer < 0.5f)
              {
                rollFlag = true;
              }

              rollInputTimer = 0;
            }
          }

          private void HandleCombatInput(float delta) 
          {

            if(rb_input)
            {
                playerCombatManager.HandleRBAction();
            }

            if(rt_input)
            {
              playerCombatManager.HandleHeavyAttack(playerInventoryManagerManager.rightWeapon);
            }

            if(lt_input)
            {
              if (twoHandFlag)
              {

              }
              else
              {
                playerCombatManager.HandleLTAction();
              }
            }

            if(lb_input)
            {
              playerCombatManager.HandleLBAction();
            }
            else
            {
              player.isBlocking = false;

              if (blockingCollider.blockingCollider.enabled)
              {
                blockingCollider.DisableBlockCollider();
              }
            }

          }

           private void HandleQuickSlotsInput()
          {
            if(d_Pad_Right)
            {
              playerInventoryManagerManager.ChangeRightWeapon();
            }
            else if(d_Pad_Left)
            {
              playerInventoryManagerManager.ChangeLeftWeapon();
            }
          }

          private void HandleInventoryInput()
          {
            if(inventory_Input)
            {
              inventoryFlag = !inventoryFlag;

              if(inventoryFlag)
              {
                uIManager.OpenSelectWindow();
                uIManager.UpdateUI();
                uIManager.hudWindow.SetActive(false);
                uIManager.CloseLevelUpPWindow();
              }
              else
              {
                uIManager.CloseSelectWindow();
                uIManager.CloseAllInventoryWindow();
                uIManager.hudWindow.SetActive(true);
                uIManager.CloseLevelUpPWindow();
              }
            }
          }

          private void HandleLockOnInput()
          {
            if (lockOnInput && lockOnFlag == false)
            {             
              lockOnInput = false;
              cameraManager.HandleLockOn();
              if (cameraManager.nearestLockOnTarget != null)
              {
                cameraManager.currentLockOnTarget = cameraManager.nearestLockOnTarget;
                lockOnFlag = true;
              }
            }
            else if (lockOnInput && lockOnFlag)
            {
              lockOnInput = false;
              lockOnFlag = false;
              cameraManager.ClearLockOnTarget();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
              right_Stick_Left_Input = false;
              cameraManager.HandleLockOn();
              if (cameraManager.leftLockTarget != null)
              {
                cameraManager.currentLockOnTarget = cameraManager.leftLockTarget;
              }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
              right_Stick_Left_Input = false;
              cameraManager.HandleLockOn();
              
              if(cameraManager.RightLockTarget != null)
              {
                cameraManager.currentLockOnTarget = cameraManager.RightLockTarget;
              }
            }

            if(cameraManager != null)
            {
              cameraManager.SetCameraHeigh();
            }
          }

          private void HandleTwoHandInput()
          {
            if(y_Input)
            {
              y_Input = false;

              twoHandFlag = !twoHandFlag;

              if(twoHandFlag)
              {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManagerManager.rightWeapon, false);
              }
              else
              {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManagerManager.rightWeapon, false);
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManagerManager.leftWeapon, true);
              }
            }
          }

          private void HandleCritivalAttackInput()
          {
            if (critical_Attack_Input)
            {
              critical_Attack_Input = false;
              playerCombatManager.AttemptBackStabOrRiposte();
            }
          }

          private void HandleUseConsumbleInput()
          {
            if (x_Input)
            {
              GameObject[] _tempFlask = GameObject.FindGameObjectsWithTag("Flask");
              int FlasksCount = _tempFlask.Length;
              if(FlasksCount <=0)
              {
                x_Input = false;
                playerInventoryManagerManager.currentConsumble.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);
              }
              else
              return;
            }
          }          
    }
}

