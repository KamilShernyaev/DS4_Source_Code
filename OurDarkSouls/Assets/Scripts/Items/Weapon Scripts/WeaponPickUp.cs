using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponPickUp : Interactable
    {
       public WeaponItem weapon;

       public override void Interact(PlayerManager playerManager)
       {
           base.Interact(playerManager);

           PickUpItem(playerManager);
       }

       private void PickUpItem(PlayerManager playerManager)
       {
           PlayerInventoryManager playerInventoryManager;
           PlayerLocomotionManager playerLocomotionManager;
           PlayerAnimatorManager playerAnimatorManager;

           playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
           playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();
           playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

           playerLocomotionManager.rigidbody.velocity = Vector3.zero;
           playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
           playerInventoryManager.weaponsInventory.Add(weapon);
           playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
           playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
           playerManager.itemInteractableGameObject.SetActive(true);
           Destroy(gameObject);
       }
    }
}
