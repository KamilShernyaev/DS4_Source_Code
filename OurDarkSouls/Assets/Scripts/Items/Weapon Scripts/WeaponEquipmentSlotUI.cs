using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        WeaponItem weapon;

        public bool rightHandSLot01;
        public bool rightHandSLot02;
        public bool leftHandSLot01;
        public bool leftHandSLot02;

        private void Awake() 
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(WeaponItem newWeapon)
        {
            if (newWeapon != null)
            {
                weapon = newWeapon;
                icon.sprite = weapon.itemIcon;
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
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    
        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlot();
            
            if(rightHandSLot01)
            {
                uIManager.rightHandSlot01Selected = true;
            }
            else if(rightHandSLot02)
            {
                uIManager.rightHandSlot02Selected = true;
            }
            else if(leftHandSLot01)
            {
                uIManager.leftHandSlot01Selected = true;
            }
            else
            {
                uIManager.leftHandSlot02Selected = true;
            }
            
            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }
    }
}
