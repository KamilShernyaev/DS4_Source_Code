using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class BodyEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        BodyEquipment item;

        public void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(BodyEquipment newItem)
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
            if(uIManager.bodyEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentBodyEquipment != null)
                {
                    uIManager.player.playerInventoryManager.bodyEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentBodyEquipment);
                }
                uIManager.player.playerInventoryManager.currentBodyEquipment = item;
                uIManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(item);
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