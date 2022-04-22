using BlackCat.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Black Cat/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {       
        [SerializeField] List<ProgressionCharacterClass> characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
            
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();


            float[] levels = lookupTable[characterClass][stat];
            if (levels.Length < level)
            {
                return 0f;
            }            
            return levels[level- 1];
        }

        private void BuildLookUp()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach(ProgressionCharacterClass progression in this.characterClasses)
            {               
              Dictionary<Stat,float[]> statLookupTable = new Dictionary<Stat,float[]>(); 

                foreach (ProgressionStat progressionStat in progression.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels; 
                }
                lookupTable[progression.characterClass] = statLookupTable;
            }        
        }

        public int GetLevels(Stat stat,CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        [Serializable]
        internal class ProgressionCharacterClass
        {
            [SerializeField] internal CharacterClass characterClass;
            [SerializeField] internal ProgressionStat[] stats;
        }
        [Serializable]
        internal class ProgressionStat
        {
            [SerializeField] internal Stat stat;
            [SerializeField] internal float[] levels;

        }
    }
}