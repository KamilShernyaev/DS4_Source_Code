using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class BodyModelChanger : MonoBehaviour
    {
        public List<GameObject> bodyModels;

        private void Awake() 
        {
            GetAllBodyModels();
        }
        private void GetAllBodyModels()
        {
            int childrenGameObjects = transform.childCount;

            for(int i = 0; i < childrenGameObjects; i++)
            {
                bodyModels.Add(transform.GetChild(i).gameObject);
            }
        }
    
        public void UnEquipAllBodyModels()
        {
            foreach(GameObject bodyModel in bodyModels)
            {
                bodyModel.SetActive(false);
            }
        }

        public void EquipModelByName(string bodyName)
        {
            for(int i = 0; i < bodyModels.Count; i++)
            {
                if(bodyModels[i].name == bodyName)
                {
                    bodyModels[i].SetActive(true);
                }
            }
        }
    }
}
