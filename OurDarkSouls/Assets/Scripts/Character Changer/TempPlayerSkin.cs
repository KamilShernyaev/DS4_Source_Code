using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  SG
{
    [CreateAssetMenu(menuName = "OurDarkSouls/TempPlayerSkin")]
    public class TempPlayerSkin : ScriptableObject 
    {
        [Header("Weapons")]
        public WeaponItem tempPrimaryWeapon;
        public WeaponItem tempOffHandWeapon;
        //public WeaponItem secondaryWeapon;

        [Header("Armor")]
        public HelmetEquipment tempHelmetEquipment;
        public BodyEquipment tempBodyEquipment;
        public LegEquipment tempLegEquipment;
        public HandEquipment tempHandEquipment;

        public int healthLevel;
        public int staminaLevel;
        public int focusLevel;

        //[Header("Spell Item")]
        //public SpellItem startingSpell;

    }
}
