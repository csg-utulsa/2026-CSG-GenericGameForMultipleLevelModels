using RPG.Utils;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        //the range of possible level the player can be
        [SerializeField] int startingLevel = 1;
        //character level
        [SerializeField] CharacterClass characterClass;
        //The type of class the character is, refrenced enum from CharacterClass file
        [SerializeField] Progression progression = null;
        //set the progression aspect in the editor
        [SerializeField] GameObject levelUpParticleEffect = null;
        //set the particle effect to be generated upon leveling up
        [SerializeField] bool shouldUseModifiers = false;
        //sets if any stat modifiers should be used.

        public event Action onLevelUp;
        //action to be called on level up

        LazyValue<int> currentLevel;
        //initialize the level at an invalid value, to ensure proper initialization
        Experience experience;
        //refrence to the expierence component

        private void Awake()
        {
            experience = GetComponent<Experience>();
            //set in awake so it can be refrenced on start
            currentLevel = new LazyValue<int>(CalculateLevel);
            //passes CalculateLevel to the lazy value, doesnt call on awake but passes the value for when it does initalize
        }

        private void Start()
        {
            currentLevel.ForceInit();
            //if current level hasnt been accessed yet, this forces it to initialize
        }

        private void OnEnable()
        //called before start but after awake
        {
            if (experience != null)
            {
                experience.onExpierenceGained += UpdateLevel;
                //adds update level to the list of methods activated on the action
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExpierenceGained -= UpdateLevel;
                //removes update level to the list of methods activated on the action
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            //checks to see if we have a new level
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                //set the new level as current
                LevelUpEffect();
                //spawn the level up effect
                onLevelUp();
                //Update anything based on level up event
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
            //returns the stat plus your additive multipliers, multiplied by your percentage modifiers
            
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
            //calls the GetStat() method from Progression and returns the float, this chain refrence prevents circular dependancies
            //returns the base stat value for the characters class, stat and level
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) {return 0; }
            //if the object should not use modifiers, return 0 bonus

            float total = 0;
            //initialize the calculation at 0
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
                //for each modifier provider in the list of modifier components (example, each item equipped w/ stat buff)
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                    //for each modifier in the provider component (example, the quantity that the statistic is changed by.)
                {
                    total += modifier;
                    //add the modifiers to the total moddifier effect
                }
            }
            return total;
            //returns the total additive modifiers for the stat
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) { return 0; }
            //if the object should not use modifiers, return 0 bonus

            float total = 0;
            //initialize the calculation at 0
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            //for each modifier provider in the list of modifier components (example, each item equipped w/ stat buff)
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                //for each modifier in the provider component (example, the quantity that the statistic is changed by.)
                {
                    total += modifier;
                    //add the modifiers to the total moddifier effect
                }
            }
            return total;
            //returns the total additive modifiers for the stat
        }

        private int CalculateLevel() 
        {
            Experience experience = GetComponent<Experience>();
            //refrence to the expierence component
            if(experience == null) { return startingLevel; }
            //for enemies that dont level up

            float currentXP = experience.GetExperience();
            //gets the players current expierence
            int maxLevelPossible = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            //gets how many possible level there are
            for (int level = 1; level <= maxLevelPossible; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                    //gets the xp required to level up
                if (XPToLevelUp > currentXP)
                {
                    return level;
                    //returns the current level
                }
            }
            return maxLevelPossible + 1;
        }
    }
}
