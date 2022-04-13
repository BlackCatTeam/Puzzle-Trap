using BlackCat.Core;
using System;
using UnityEngine;

namespace BlackCat.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Black Cat/Weapons/Make New Weapon",order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float range = 0f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (this.equippedPrefab != null)
            {                
               GameObject weapon = Instantiate(this.equippedPrefab, GetHandTransform(rightHand,leftHand));
                weapon.name = weaponName;
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage);
        }

        public float GetDamage() { return this.damage; }
        public float GetRange() { return this.range; }
        public float GetTimeBetweenAttacks() { return this.timeBetweenAttacks; }
        

    }
}