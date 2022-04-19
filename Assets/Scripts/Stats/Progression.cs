using BlackCat.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Black Cat/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {       
        [SerializeField] List<ProgressionCharacterClass> characterClasses;
            
        [Serializable]
        internal class ProgressionCharacterClass
        {  
            [SerializeField] internal CharacterClass characterClass;
            [SerializeField] internal float[] health;
        }  
        public float GetHealth(CharacterClass characterClass, int level)
        {
            level = level - 1;
            foreach(ProgressionCharacterClass progression in this.characterClasses)
            {
                if (progression.characterClass == characterClass)
                {
                    if (level > progression.health.Length)
                        level = progression.health.Length;
                    else if (level < 0)
                        level = 0;                
                    return progression.health[level];
                }
            }
            return 0f;
        } 
    }
}