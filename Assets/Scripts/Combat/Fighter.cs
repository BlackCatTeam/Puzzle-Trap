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
        [SerializeField] WeaponConfig defaultWeaponConfig = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        private Mover mover;
        private bool IsAttacking =false;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            currentWeaponConfig = defaultWeaponConfig;
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

            

            if (!GetIsInRange(target.transform) )
            {
                mover.MoveTo(target.transform.position,1f);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }

        }

        private Weapon SetupDefaultWeapon() => AttachWeapon(defaultWeaponConfig);
            
        
        public Health GetActualTarget() =>  target;
        
        public void EquipWeapon(WeaponConfig weapon)
        {
           currentWeaponConfig = weapon;                
           currentWeapon.value =  AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {            
            return weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (combatTarget == this.gameObject) return false;
            if (!mover.CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform))
            {
                return false;
            }
                

            Health verifytarget = combatTarget.GetComponent<Health>();            
            return verifytarget != null && !verifytarget.IsDead();
            
        }


        // Animation Event Hit()
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
                currentWeapon.value.OnHit();
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(gameObject, rightHandTransform, leftHandTransform, target, damage);
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
            if (timeSinceLastAttack > currentWeaponConfig.GetTimeBetweenAttacks() )
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;

            }
        }

        private void TriggerAttack()
        {
            IsAttacking = true;
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // This will trigger the Hit() Animation Event Above
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange(Transform targetTrasnform)
        {
            return Vector3.Distance(this.transform.position, targetTrasnform.position) <= currentWeaponConfig.GetRange();
        }

        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
            mover.Cancel();
        }

        private void TriggerStopAttack()
        {
            IsAttacking = false;
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
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<(float valueBonus, float percentageBonus)> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return (currentWeaponConfig.GetDamage(),currentWeaponConfig.getPercentageBonus());
            }
        }
    }
}
