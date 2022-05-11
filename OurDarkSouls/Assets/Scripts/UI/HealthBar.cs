using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        private void Start() 
        {
            slider = GetComponent<Slider>();    
        }

        public void SetMaxHealth(int maxHelth)
        {
            slider.maxValue = maxHelth;
            slider.value = maxHelth;
        }

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}
