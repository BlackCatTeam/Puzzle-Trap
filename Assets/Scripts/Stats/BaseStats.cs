using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Stats {
	public class BaseStats : MonoBehaviour
	{
		
		[SerializeField, Range(1, 99)] private int startingLevel = 1;
		[SerializeField] private CharacterClass characterClass;
		[SerializeField] Progression progression;

		public int GetStartingLevel()
        {
			return this.startingLevel;
        }
		public float GetHealth()
        {
			return progression.GetHealth(characterClass,startingLevel);
        }
		public CharacterClass GetClass()
        {
			return this.characterClass;
        }

		public float GetExperienceReward()
        {
			return 10f;
        }
	
	}
}
