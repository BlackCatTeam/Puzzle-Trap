using BlackCat.Attributes;
using BlackCat.Core;
using BlackCat.Core.Interfaces;
using BlackCat.Movement;
using BlackCat.Saving;
using UnityEngine;

namespace BlackCat.Combat {
	public class Fighter : MonoBehaviour , IAction , ISaveable
    {             
        [SerializeField] Health target;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        Weapon currentWeapon;
        private Mover mover;        
        float timeSinceLastAttack = Mathf.Infinity;
        

        private void Start()
        {
            mover = GetComponent<Mover>();
            if (currentWeapon == null)
                EquipWeapon(defaultWeapon);
        }

        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            //Cannot Attack Nothing
            if (target == null) return;
            // Cannot Attack a Dead Enemy
            if (target.IsDead()) return;
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
        public Health GetActualTarget()
        {
            return target;
        }
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform,leftHandTransform, GetComponent<Animator>());
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (combatTarget == this.gameObject) return false;

            Health verifytarget = combatTarget.GetComponent<Health>();            
            return verifytarget != null && !verifytarget.IsDead();
            
        }


        // Animation Event Hit()
        void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(gameObject, rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(gameObject,currentWeapon.GetDamage());
            }
        }

        void Shoot()
        {
            Hit();
        }
        private void AttackBehavior()
        {
            this.transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeapon.GetTimeBetweenAttacks() )
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
            return Vector3.Distance(this.transform.position, target.transform.position) <= currentWeapon.GetRange();
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
