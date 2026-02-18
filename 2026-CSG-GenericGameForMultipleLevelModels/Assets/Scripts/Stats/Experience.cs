using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExpierenceGained;
        //created a event for on expierence gained

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            //adds the passed experience to the experience total
            onExpierenceGained();
        }
        public float GetExperience()
        {
            return experiencePoints;
            //a public method to refrence amount of experience points
        }
    }
}
