using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class LegEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        LegEquipment item;

        public void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(LegEquipment newItem)
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
            if(uIManager.legEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentLegEquipment != null)
                {
                    uIManager.player.playerInventoryManager.legEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentLegEquipment);
                }
                uIManager.player.playerInventoryManager.currentLegEquipment = item;
                uIManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
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