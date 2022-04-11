using UnityEngine;

namespace BlackCat.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Black Cat/Weapons/Make New Weapon",order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float weaponRange = 0f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private bool IsRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (this.equippedPrefab != null)
            {
                Transform handTransform = IsRightHanded ? rightHand : leftHand;
                Instantiate(this.equippedPrefab, handTransform);
            }
            if (this.animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride; 
            }
        }
            
        public float GetDamage() { return this.damage; }
        public float GetRange() { return this.weaponRange; }
        public float GetTimeBetweenAttacks() { return this.timeBetweenAttacks; }
        

    }
}