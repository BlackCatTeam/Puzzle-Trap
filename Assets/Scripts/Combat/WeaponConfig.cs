using BlackCat.Attributes;
using BlackCat.Core;
using BlackCat.Inventories;
using BlackCat.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Black Cat/Weapons/Make New Weapon",order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Weapon equippedPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float percentageDamageBonus = 0f;
        [SerializeField] private float range = 0f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Weapon weapon = null;
            DestroyOldWeapon(rightHand, leftHand);
            if (this.equippedPrefab != null)
            {                
                weapon = Instantiate(this.equippedPrefab, GetHandTransform(rightHand,leftHand));
                weapon.gameObject.name = weaponName;                
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            else if(overrideController != null)
            {                                
               animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;                
            }

            return weapon;
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(GameObject instigator,Transform rightHand, Transform leftHand, Health target,float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator, target, calculatedDamage);
        }

        public float GetDamage() { return this.damage; }
        public float getPercentageBonus() { return this.percentageDamageBonus; }
        public float GetRange() { return this.range; }
        public float GetTimeBetweenAttacks() { return this.timeBetweenAttacks; }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return damage;
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return percentageDamageBonus;
        }
    }
}