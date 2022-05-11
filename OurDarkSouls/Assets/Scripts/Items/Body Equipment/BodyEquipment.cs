using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{    
    [CreateAssetMenu(menuName = "Items/Equipment/Body Equipment")]
    public class BodyEquipment : EquipmentItem
    {
        public string bodyModelName;
        public string upperLeftArmModelName;
        public string upperRightArmModelName;
    }
}
