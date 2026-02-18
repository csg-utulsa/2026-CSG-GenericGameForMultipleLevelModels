using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
//adds a new interface option into the editor when you right click, can now create new progression scriptable object
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;
    //select the character class from the editor to mark what progression tree the object will follow

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        float[] levels = lookupTable[characterClass][stat];
        //refrence the stat lookup table for the character class, then stat
        if (levels.Length <level) { return 0; }
        //pulls the length of the levels array out to see if we are attempting to index a value out of range
        return levels[level - 1];
        //returns the value assigned to the level's associated index for the desired character class and stat
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();
        //makes sure the lookup table is built
        float[] levels = lookupTable[characterClass][stat];
        //refrence the stat lookup table for the character class, then stat
        return levels.Length;
    }

    private void BuildLookup()
    {
        if(lookupTable != null) { return; }
        //if the table is built, ignore

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
        //creates the new lookupTable Dictionary, character class as key, then a dictionary as its value

        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        //for each character class
        {
            var statLookupTable = new Dictionary<Stat, float[]>();
            //the statLookupTable Dictionary

            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levels;
                //adds the levels values into the stat look up table stat as the key, levels as the values
            }

            lookupTable[progressionClass.characterClass] = statLookupTable;
            //adds the character class as the key, and the stat table as the value
        }
    }

    [System.Serializable]
    //marks the ProgressionCharacterClass as serializable, and makes the classes appear in the editor
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        //select the character class
        public ProgressionStat[] stats;
        //shows the list of stats
        //public float[] health;
        //health array for health values as object levels up
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        //tells us what stat it is
        public float[] levels;
        //adds the level array for the stat
    }
}