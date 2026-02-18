using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Text levelText;
        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            //cache the players BaseStats component
            levelText = GetComponent<Text>();
        }

        private void Update()
        {
            levelText.text = String.Format("{0:0}", baseStats.GetLevel());
            //updates the Level text on the UI to the current level
        }
    }

}