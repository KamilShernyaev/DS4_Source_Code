using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class TempChangeSkinPlayer : MonoBehaviour
    {
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;
        public TempPlayerSkin tempPlayerSkin;
        
        void Awake()
        {
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

            playerInventoryManager.currentHelmetEquipment =  tempPlayerSkin.tempHelmetEquipment;
            playerInventoryManager.currentBodyEquipment =  tempPlayerSkin.tempBodyEquipment;
            playerInventoryManager.currentLegEquipment =  tempPlayerSkin.tempLegEquipment;
            playerInventoryManager.currentHandEquipment =  tempPlayerSkin.tempHandEquipment;

            playerStatsManager.healthLevel = tempPlayerSkin.healthLevel;
            playerStatsManager.focusLevel = tempPlayerSkin.focusLevel;
            playerStatsManager.staminaLevel = tempPlayerSkin.staminaLevel;

            
        }
    }
}
