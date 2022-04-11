using BlackCat.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core {	
	public class Health : MonoBehaviour , ISaveable
	{
		[SerializeField]
		[Min(0)]
		float healthPoints = 100f;
		bool isDead = false;
		public void TakeDamage(float damage)
        {
			healthPoints = Mathf.Max(healthPoints - damage, 0);
			VerifyDeath();
		}

		public bool IsDead() { return isDead; }
		 
		private void VerifyDeath()
        {
			if (IsDead()) return;

			if (healthPoints <= 0f)
			{
				isDead = true;
				GetComponent<Animator>().SetTrigger("die");
				GetComponent<ActionScheduler>().CancelCurrentAction();
			}
        }

		public object CaptureState()
		{
			return healthPoints;
		}

        public void RestoreState(object state)
        {
			this.healthPoints = (float)state;
			VerifyDeath();
		}
    }
}
