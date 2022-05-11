using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class ClassSelector : MonoBehaviour
    {
        PlayerManager player;
        public TempPlayerSkin tempPlayerSkin;

        [Header("Class Info UI")]
        public Text hpStat;
        public Text staminaStat;
        public Text focusStat;
        public Text classDescription;


        [Header("Class Starting Statas")]
        public ClassStats[] classStats;


        [Header("Class Starting Gear")]
        public ClassGear[] classGears;

        private void Awake() 
        {
            player = FindObjectOfType<PlayerManager>();
        }

        private void AssignClassStats(int classChosen)
        {
            player.playerStatsManager.healthLevel = classStats[classChosen].maxHpLevel;
            tempPlayerSkin.healthLevel = classStats[classChosen].maxHpLevel;

            player.playerStatsManager.staminaLevel = classStats[classChosen].maxStaminaLevel;
            tempPlayerSkin.staminaLevel = classStats[classChosen].maxStaminaLevel;

            player.playerStatsManager.focusLevel = classStats[classChosen].maxFocusLevel;
            tempPlayerSkin.focusLevel = classStats[classChosen].maxFocusLevel;

            classDescription.text = classStats[classChosen].classDescription;
        }

        public void AssignKnightClass()
        {
            AssignClassStats(0);
            player.playerInventoryManager.currentHelmetEquipment = classGears[0].helmetEquipment;
            tempPlayerSkin.tempHelmetEquipment = classGears[0].helmetEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[0].bodyEquipment;
            tempPlayerSkin.tempBodyEquipment = classGears[0].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[0].legEquipment;
            tempPlayerSkin.tempLegEquipment = classGears[0].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[0].handEquipment;
            tempPlayerSkin.tempHandEquipment = classGears[0].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[0].primaryWeapon;
            tempPlayerSkin.tempPrimaryWeapon = classGears[0].primaryWeapon;
            //player.playerInventoryManager.weaponsInRightHandSlots[1] = classGears[0].secondaryWeapon; - Secondary slot
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[0].offHandWeapon;
            tempPlayerSkin.tempOffHandWeapon = classGears[0].offHandWeapon;

            player.playerEquipmentManager.EquipAllEquipmentModels();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            hpStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text =  player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
        }
        public void AssignNakedClass()
        {
            AssignClassStats(1);
            player.playerInventoryManager.currentHelmetEquipment = classGears[1].helmetEquipment;
            tempPlayerSkin.tempHelmetEquipment = classGears[1].helmetEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[1].bodyEquipment;
            tempPlayerSkin.tempBodyEquipment = classGears[1].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[1].legEquipment;
            tempPlayerSkin.tempLegEquipment = classGears[1].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[1].handEquipment;
            tempPlayerSkin.tempHandEquipment = classGears[1].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[1].primaryWeapon;
            tempPlayerSkin.tempPrimaryWeapon = classGears[1].primaryWeapon;
            //player.playerInventoryManager.weaponsInRightHandSlots[1] = classGears[0].secondaryWeapon; - Secondary slot
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[1].offHandWeapon;
            tempPlayerSkin.tempOffHandWeapon = classGears[1].offHandWeapon;

            player.playerEquipmentManager.EquipAllEquipmentModels();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();
            
            hpStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text =  player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
        }

        public void AssignWizardClass()
        {
            AssignClassStats(2);
            player.playerInventoryManager.currentHelmetEquipment = classGears[2].helmetEquipment;
            tempPlayerSkin.tempHelmetEquipment = classGears[2].helmetEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[2].bodyEquipment;
            tempPlayerSkin.tempBodyEquipment = classGears[2].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[2].legEquipment;
            tempPlayerSkin.tempLegEquipment = classGears[2].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[2].handEquipment;
            tempPlayerSkin.tempHandEquipment = classGears[2].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[2].primaryWeapon;
            tempPlayerSkin.tempPrimaryWeapon = classGears[2].primaryWeapon;
            //player.playerInventoryManager.weaponsInRightHandSlots[1] = classGears[0].secondaryWeapon; - Secondary slot
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[2].offHandWeapon;
            tempPlayerSkin.tempOffHandWeapon = classGears[2].offHandWeapon;

            player.playerEquipmentManager.EquipAllEquipmentModels();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();
            
            hpStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text =  player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
        }

        public void AssignBarbarianClass()
        {
            AssignClassStats(3);
            player.playerInventoryManager.currentHelmetEquipment = classGears[3].helmetEquipment;
            tempPlayerSkin.tempHelmetEquipment = classGears[3].helmetEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGears[3].bodyEquipment;
            tempPlayerSkin.tempBodyEquipment = classGears[3].bodyEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[3].legEquipment;
            tempPlayerSkin.tempLegEquipment = classGears[3].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[3].handEquipment;
            tempPlayerSkin.tempHandEquipment = classGears[3].handEquipment;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[3].primaryWeapon;
            tempPlayerSkin.tempPrimaryWeapon = classGears[3].primaryWeapon;
            //player.playerInventoryManager.weaponsInRightHandSlots[1] = classGears[0].secondaryWeapon; - Secondary slot
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[3].offHandWeapon;
            tempPlayerSkin.tempOffHandWeapon = classGears[3].offHandWeapon;

            player.playerEquipmentManager.EquipAllEquipmentModels();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();
            
            hpStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text =  player.playerStatsManager.staminaLevel.ToString();
            focusStat.text = player.playerStatsManager.focusLevel.ToString();
        }
    }
}
