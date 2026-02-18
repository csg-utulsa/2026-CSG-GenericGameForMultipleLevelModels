using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text healthText;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            //cache the players health component
            healthText = GetComponent<Text>();
        }

        private void Update()
        {
            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(),health.GetMaxHealthPoints());
            //updates the health text on the UI to the current health remaining out of max
            //{0:0} gives the number with no decimal places, {0:0:0} yeilds one decimal place and so on
            //the / displays a slash, the {1:0} displays a second number formated to no decimal places
        }
    }

}