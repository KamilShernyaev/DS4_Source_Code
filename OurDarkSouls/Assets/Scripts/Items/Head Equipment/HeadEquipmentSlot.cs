using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{    
    public class HeadEquipmentSlot : MonoBehaviour
    {
        UIManager uIManager;
        public Image icon;
        HelmetEquipment item;

        private void Awake() 
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(HelmetEquipment hemletItem)
        {
            if (hemletItem != null)
            {
                this.item = hemletItem;
                icon.sprite = this.item.itemIcon;
                icon.enabled = true;
                this.gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }

        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    
        public void SelectThisSlot()
        {
            uIManager.headEquipmentSlotSelected = true;
        }
    }
}
