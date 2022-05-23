using BlackCat.Attributes;
using BlackCat.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Stats {
	public class BaseStats : MonoBehaviour
	{

		[SerializeField, Range(1, 99)] private int startingLevel = 1;
		[SerializeField] private CharacterClass characterClass;
		[SerializeField] Progression progression;
		[SerializeField] GameObject levelUpEffect;

		public event Action OnLevelUp;
		LazyValue<int> currentLevel;

		Experience experience;
        private void Awake()
        {
			experience = GetComponent<Experience>();
			currentLevel = new LazyValue<int>(CalculateLevel);

		}
        private void Start()
        {
			currentLevel.ForceInit();
        }

        private void OnEnable()
		{
			if (experience != null)
				experience.onExpGain += UpdateLevel;
			OnLevelUp += LevelUpEffect;

		}
		private void OnDisable()
        {
			if (experience != null)
				experience.onExpGain -= UpdateLevel;
		}
        public int GetStartingLevel()
        {
			return this.startingLevel;
        }
		public float GetStat(Stat stat)
        {
		float bonusAdditive = GetAdditiveModifier(stat);
		float bonusPercentage = GetPercentageModifier(stat);

			return GetBaseStat(stat) + bonusAdditive * (1 + bonusPercentage / 100);
        }
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
			float totalAdditive = 0;
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
				foreach (var modifier  in provider.GetAdditiveModifier(stat))
                {
					totalAdditive += modifier;					
                }
            }
			return totalAdditive;
        }
		private float GetPercentageModifier(Stat stat)
		{
			float totalPercentage = 0;
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach (var modifier in provider.GetPercentageModifier(stat))
				{
					totalPercentage += modifier;
				}
			}



			return totalPercentage;
		}
		private void UpdateLevel()
        {

			int newLevel = CalculateLevel();
			if (newLevel > currentLevel.value)
            {
				currentLevel.value = newLevel;
				OnLevelUp();
			}
		}

        private void LevelUpEffect()
        {
			Instantiate(levelUpEffect, transform);       
		}
		public CharacterClass GetClass()
		{
			return this.characterClass;
		}
		public int GetLevel()
        {			
			return currentLevel.value;
        }

        public int CalculateLevel()
        {            
			if (experience == null) return  startingLevel;

			float currentXP = experience.GetExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
			for (int level = 1; level <= penultimateLevel; level++)
			{
				float XPToLevelUP = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
				if (XPToLevelUP > currentXP)
					return  level;
            }
			return penultimateLevel + 1;
        }

	
	}
}
