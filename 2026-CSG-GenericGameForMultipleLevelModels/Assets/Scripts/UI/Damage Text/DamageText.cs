using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;
        //gets refrence to the text component in the inspector

        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
            //sets the text to a non decimal float value
        }
    }
}

