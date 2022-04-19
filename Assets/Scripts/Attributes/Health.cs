using BlackCat.Core;
using BlackCat.Saving;
using BlackCat.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Attributes {	
	public class Health : MonoBehaviour , ISaveable
	{
		[SerializeField]
		[Min(0)]
		float healthPoints = 100f;
		bool isDead = false;

        private void Start()
        {
			healthPoints = GetComponent<BaseStats>().GetHealth();
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
			healthPoints = Mathf.Max(healthPoints - damage, 0);
			VerifyDeath(instigator);
		}

		public bool IsDead() { return isDead; }
		 
		private void VerifyDeath(GameObject instigator)
        {
			if (IsDead()) return;

			if (healthPoints <= 0f)
            {
                Die();
				if (instigator != null) 
					AwardExperiece(instigator);
            }
        }

        private void AwardExperiece(GameObject instigator)
        {
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) return;

			experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage()
        {
			return (100 * healthPoints) / GetComponent<BaseStats>().GetHealth(); ;
		}
		public float GetHealth()
        {
			return this.healthPoints;
        }
		public float GetMaxHealth()
        {
			return GetComponent<BaseStats>().GetHealth();
		}
		public object CaptureState()
		{
			return healthPoints;
		}

        public void RestoreState(object state)
        {
			this.healthPoints = (float)state;
			VerifyDeath(null);
		}
    }
}