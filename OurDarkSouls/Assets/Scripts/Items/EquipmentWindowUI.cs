using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public WeaponEquipmentSlotUI[] weaponEquipmentSlotUI;
        public HeadEquipmentSlot headEquipmentSlotUI;
        public BodyEquipmentSlotUI bodyEquipmentSlotUI;
        public LegEquipmentSlotUI legEquipmentSlotUI;
        public HandEquipmentSlotUI handEquipmentSlotUI;

        public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
        {
            for( int i = 0; i < weaponEquipmentSlotUI.Length; i++)
            {
                if(weaponEquipmentSlotUI[i].rightHandSLot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlotUI[i].rightHandSLot02)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[1]);
                }
                else if(weaponEquipmentSlotUI[i].leftHandSLot01)
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    weaponEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
                }
            }
        }    
        
        public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            if (playerInventory.currentHelmetEquipment != null)
            {
                headEquipmentSlotUI.AddItem(playerInventory.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentBodyEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(playerInventory.currentBodyEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }   

            if (playerInventory.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(playerInventory.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(playerInventory.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }
        }
    }
}

