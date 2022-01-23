using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat {
	public class Health : MonoBehaviour
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
			if (healthPoints == 0f && !IsDead())
			{
				isDead = true;
				GetComponent<Animator>().SetTrigger("die");
				GetComponent<CapsuleCollider>().isTrigger = true;
			}
        }
	}
}
