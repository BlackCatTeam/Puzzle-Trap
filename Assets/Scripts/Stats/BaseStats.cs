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
			(float damageBonus, float percentageBonus) BonusStats = GetAdditiveModifier(stat);

			return GetBaseStat(stat) + BonusStats.damageBonus * (1 + BonusStats.percentageBonus / 100);
        }
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private (float valueBonus,float percentageBonus) GetAdditiveModifier(Stat stat)
        {
			float totalDamage = 0;
			float totalPercentage = 0;
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
				foreach (var modifier  in provider.GetAdditiveModifier(stat))
                {
					totalDamage += modifier.valueBonus;
					totalPercentage += modifier.percentageBonus;

                }
            }



			return (totalDamage,totalPercentage);
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
