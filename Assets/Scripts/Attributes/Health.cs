using BlackCat.Core;
using BlackCat.Saving;
using BlackCat.Stats;
using BlackCat.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlackCat.Attributes {	
	public class Health : MonoBehaviour , ISaveable
	{
		[SerializeField] float regenerationPorcentage = 70f;
		[Min(0)]
		LazyValue<float> healthPoints;

		[SerializeField] TakeDamageEvent takeDamage;
		[SerializeField] UnityEvent onDie;
		[SerializeField] UnityEvent onHeal;
		[Serializable]
		public class TakeDamageEvent : UnityEvent<float> 
		{
		}



		bool isDead = false;
        private void Awake()
        {
			healthPoints = new LazyValue<float>(GetInitialHealth);

		}
        private void Start()
        {
			healthPoints.ForceInit();
        }
        private float GetInitialHealth()
        {
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}
        private void OnEnable()
        {
			GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
		}

        internal void Heal(float healthToRestore)
        {
			healthPoints.value = Mathf.Min(healthPoints.value +healthToRestore, GetMaxHealth());
			onHeal.Invoke();
		}

		private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
		}
        private void RegenerateHealth()
        {
			float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPorcentage / 100);
			healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
			onHeal.Invoke();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
			healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
			takeDamage.Invoke(damage);
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
                {
					onDie.Invoke();
					AwardExperiece(instigator);
				}
					
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
			return (100 * GetFraction());
		}
		public float GetFraction()
        {
			return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
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