using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HandEquipmentInventorySlot : MonoBehaviour
    {        
        UIManager uIManager;
        public Image icon;
        HandEquipment item;

        public void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HandEquipment newItem)
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
            if(uIManager.handEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHandEquipment != null)
                {
                    uIManager.player.playerInventoryManager.handEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHandEquipment);
                }
                uIManager.player.playerInventoryManager.currentHandEquipment = item;
                uIManager.player.playerInventoryManager.handEquipmentInventory.Remove(item);
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else
            {
                return;
            }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlot();
        }        
    }
}