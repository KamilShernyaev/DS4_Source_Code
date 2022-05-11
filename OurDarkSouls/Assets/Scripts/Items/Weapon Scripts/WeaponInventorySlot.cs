using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        WeaponItem item;

        public void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
            icon.gameObject.SetActive(false);

        }

        public void EquipThisItem()
        {
            if(uIManager.rightHandSlot01Selected)
            {
                uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
                uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = item;
                uIManager.player. playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.rightHandSlot02Selected)
            {
                uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
                uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = item;
                uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot01Selected)
            {
                uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = item;
                uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else if(uIManager.leftHandSlot02Selected)
            {
                uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = item;
                uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
            }
            else
            {
                return;
            }

            uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.rightWeapon,false);
            uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.leftWeapon,true);  

            uIManager.equipmentWindowUI.LoadWeaponOnEquipmentScreen(uIManager.player.playerInventoryManager);       
            uIManager.ResetAllSelectedSlot();
        }
    }
}