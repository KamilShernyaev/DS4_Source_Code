using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

        [Header("Equipment Model Changers")]
        //Head Equipment
        HelmetModelChanger helmetModelChanger;

        //Body Equipment
        BodyModelChanger bodyModelChanger;

        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;

        //Leg Equipment
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;

        //Hand Equipment
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        [Header("Facial Features")]
        public GameObject[] facialFeatures;

        [Header("Defalt Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedBodyModel;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        public BlockingCollider blockingCollider;

        private void Awake() 
        {
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();   
            playerStatsManager = GetComponent<PlayerStatsManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            bodyModelChanger = GetComponentInChildren<BodyModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
        }

        private void Start() 
        {
            EquipAllEquipmentModels();
        }

        public void EquipAllEquipmentModels()
        {
             //HELMET EQUIPMENT
            helmetModelChanger.UnEquipAllHelmetModels();

            if(playerInventoryManager.currentHelmetEquipment != null)
            {
                if(playerInventoryManager.currentHelmetEquipment.hideFacialFeatures)
                {
                    foreach (var feature in facialFeatures)
                    {
                        feature.SetActive(false);
                    }
                }
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsoptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
                Debug.Log("Head Absorption is" + playerStatsManager.physicalDamageAbsoptionHead + "%");
            }
            else
            {
                nakedHeadModel.SetActive(true);
                playerStatsManager.physicalDamageAbsoptionHead = 0;

                foreach (var feature in facialFeatures)
                    {
                        feature.SetActive(true);
                    }
            }

             //BODY EQUIPMENT
            bodyModelChanger.UnEquipAllBodyModels();
            upperLeftArmModelChanger.UnEquipAllModels();
            upperRightArmModelChanger.UnEquipAllModels();

            if(playerInventoryManager.currentBodyEquipment != null)
            {
                bodyModelChanger.EquipModelByName(playerInventoryManager.currentBodyEquipment.bodyModelName);
                upperLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipModelByName(playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
                playerStatsManager.physicalDamageAbsoptionBody = playerInventoryManager.currentBodyEquipment.physicalDefense;
                Debug.Log("Body Absorption is" + playerStatsManager.physicalDamageAbsoptionBody + "%");
            }
            else
            {
                bodyModelChanger.EquipModelByName(nakedBodyModel);
                upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
                playerStatsManager.physicalDamageAbsoptionBody = 0;
            }

             //LEG EQUIPMENT
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();

            if(playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipModelByName(playerInventoryManager.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipModelByName(playerInventoryManager.currentLegEquipment.rightLegName);
                playerStatsManager.physicalDamageAbsoptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
                Debug.Log("Hip Absorption is" + playerStatsManager.physicalDamageAbsoptionLegs + "%");
            } 
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipModelByName(nakedRightLeg);
                playerStatsManager.physicalDamageAbsoptionLegs = 0;
            }

            //HAND EQUIPMENT
            lowerLeftArmModelChanger.UnEquipAllModels();
            lowerRightArmModelChanger.UnEquipAllModels();
            leftHandModelChanger.UnEquipAllModels();
            rightHandModelChanger.UnEquipAllModels();

            if(playerInventoryManager.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
                Debug.Log("Hand Absorption is" + playerStatsManager.physicalDamageAbsoptionLegs + "%");
            } 
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
            }
        }

        public void OpenBlockindCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);

            }

            blockingCollider.EnableBlockCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockCollider();
        }
    }

}
