using BlackCat.Core;
using BlackCat.Core.Interfaces;
using BlackCat.Movement;
using UnityEngine;

namespace BlackCat.Combat {
	public class Fighter : MonoBehaviour , IAction
    {
        [SerializeField]
		float WeaponRange = 2f;
        [SerializeField]
        float WeaponDamage = 5f;
        [SerializeField]
        Health target;
        Mover mover;
        [SerializeField]
        float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
             mover = GetComponent<Mover>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            //Cannot Attack Nothing
            if (target == null) return;
            // Cannot Attack a Dead Enemy
            if (target.IsDead()) {return; }
            // Cannot Attack Himself
            if (target == this) return;



            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position,1f);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
            
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (combatTarget == this.gameObject) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();            
            return targetToTest != null && !targetToTest.IsDead();
            
        }


        // Animation Event Hit()
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(WeaponDamage);
        }

        private void AttackBehavior()
        {
            this.transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks )
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // This will trigger the Hit() Animation Event Above
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(this.transform.position, target.transform.position) <= WeaponRange;
        }

        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
            mover.Cancel();
        }

        private void TriggerStopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");

            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.GetComponent<Health>();

        }

	}
}
