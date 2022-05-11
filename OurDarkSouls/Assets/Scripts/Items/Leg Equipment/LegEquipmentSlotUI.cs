using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class LegEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        LegEquipment Item;

        private void Awake() 
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(LegEquipment legEquipment)
        {
            if (legEquipment != null)
            {
                Item = legEquipment;
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
            uIManager.legEquipmentSlotSelected = true;
        }
    }
}