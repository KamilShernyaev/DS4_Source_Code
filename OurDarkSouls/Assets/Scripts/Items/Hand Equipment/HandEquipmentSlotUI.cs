using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {        
        UIManager uIManager;
        public Image icon;
        HandEquipment Item;

        private void Awake() 
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(HandEquipment handEquipment)
        {
            if (handEquipment != null)
            {
                Item = handEquipment;
                icon.sprite = Item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }

        public void ClearItem()
        {
            Item = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    
        public void SelectThisSlot()
        {
            uIManager.handEquipmentSlotSelected = true;
        }
    }
}
