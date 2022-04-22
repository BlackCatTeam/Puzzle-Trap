using BlackCat.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Stats {
	public class Experience : MonoBehaviour,ISaveable
	{
		[SerializeField] float experiencePoints = 0f;


        public event Action onExpGain;

        public object CaptureState()
        {
            return experiencePoints;
        }
        public float GetExperience()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
			experiencePoints += experience;
            onExpGain();
        }

        public void RestoreState(object state)
        {
            this.experiencePoints = (float)state;
        }
    }
}
