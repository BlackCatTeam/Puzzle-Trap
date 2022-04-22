using BlackCat.Core;
using BlackCat.Saving;
using BlackCat.Stats;
using BlackCat.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Attributes {	
	public class Health : MonoBehaviour , ISaveable
	{
		[SerializeField] float regenerationPorcentage = 70f;
		[Min(0)]
		LazyValue<float> healthPoints;
		bool isDead = false;
        private void Awake()
        {
			healthPoints = new LazyValue<float>(GetInitialHealth);

		}
		private float GetInitialHealth()
        {
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}
        private void OnEnable()
        {
			GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
		}
        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
		}
        private void RegenerateHealth()
        {
			float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPorcentage / 100);
			healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
			print(gameObject.name + "took damage " + damage);
			healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
			VerifyDeath(instigator);
		}

		public bool IsDead() { return isDead; }
		 
		private void VerifyDeath(GameObject instigator)
        {
			if (IsDead()) return;

			if (healthPoints.value <= 0f)
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

			experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }


        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetHealthPercentage()
        {
			if (healthPoints.value < 0) return 0f;
			return (100 * healthPoints.value) / GetComponent<BaseStats>().GetStat(Stat.Health); ;
		}
		public float GetHealth()
        {
			return this.healthPoints.value;
        }
		public float GetMaxHealth()
        {
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}
		public object CaptureState()
		{
			return healthPoints.value;
		}

        public void RestoreState(object state)
        {
			this.healthPoints.value = (float)state;
			VerifyDeath(null);
		}
    }
}