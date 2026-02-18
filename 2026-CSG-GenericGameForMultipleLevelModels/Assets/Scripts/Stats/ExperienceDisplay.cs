using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        Text experienceText;
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            //cache the players experience component
            experienceText = GetComponent<Text>();
        }

        private void Update()
        {
            experienceText.text = String.Format("{0:0}", experience.GetExperience());
            //updates the health text on the UI to the current percentage of health remaining
            //{0:0} gives the number with no decimal places, {0:0:0} yeilds one decimal place and so on
        }
    }

}