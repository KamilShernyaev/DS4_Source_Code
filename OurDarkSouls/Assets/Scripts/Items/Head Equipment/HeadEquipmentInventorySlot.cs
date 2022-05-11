using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HeadEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        HelmetEquipment item;

        public void Awake()
        {
            uIManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HelmetEquipment newItem)
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
            if(uIManager.headEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    uIManager.player.playerInventoryManager.helmetEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHelmetEquipment);
                }
                uIManager.player.playerInventoryManager.currentHelmetEquipment = item;
                uIManager.player.playerInventoryManager.helmetEquipmentInventory.Remove(item);
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
