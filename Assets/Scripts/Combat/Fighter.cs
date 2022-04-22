using BlackCat.Attributes;
using BlackCat.Core;
using BlackCat.Core.Interfaces;
using BlackCat.Movement;
using BlackCat.Saving;
using BlackCat.Stats;
using BlackCat.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat {
	public class Fighter : MonoBehaviour , IAction , ISaveable, IModifierProvider
    {             
        [SerializeField] Health target;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        LazyValue<Weapon> currentWeapon;
        private Mover mover;        
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }



        private void Start()
        {
            currentWeapon.ForceInit();
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
        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;                
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            if (currentWeapon == null) return;
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(gameObject, rightHandTransform, leftHandTransform, target, damage);
            }
            else
            {
                target.TakeDamage(gameObject,damage);
            }
        }

        void Shoot()
        {
            Hit();
        }
        private void AttackBehavior()
        {
            this.transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeapon.value.GetTimeBetweenAttacks() )
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
            return Vector3.Distance(this.transform.position, target.transform.position) <= currentWeapon.value.GetRange();
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
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<(float valueBonus, float percentageBonus)> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return (currentWeapon.value.GetDamage(),currentWeapon.value.getPercentageBonus());
            }
        }
    }
}
