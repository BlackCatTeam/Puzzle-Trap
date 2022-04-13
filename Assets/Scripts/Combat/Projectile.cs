using BlackCat.Core;
using UnityEngine;

namespace BlackCat.Combat {
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] float baseProjectileDamage = 0f;
        [SerializeField] bool IsRoaming = true;
        [SerializeField] GameObject hitEffect = null;
        Health target = null;
        float damage = 0;
        [SerializeField] float maxLifeTime = 3f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] GameObject[] destroyOnHit;
        private void Start()
        {
            LookAtTarget();
            Destroy(gameObject, maxLifeTime);
        }

        private void LookAtTarget()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;

            if (IsRoaming && !target.IsDead())
                LookAtTarget();

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage + baseProjectileDamage;
            
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void TakeDamage(Health _target)
        {
            speed = 0f;

            _target.TakeDamage(damage);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(),transform.rotation);
            }
            DestroyProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.IsDead()) 
            {
                Health newTarget = other.gameObject.GetComponent<Health>();
                if (newTarget != null && !newTarget.IsDead())
                {
                    TakeDamage(newTarget);

                }
                return;
            }

            if (other.GetComponent<Health>() != target) return;
            TakeDamage(this.target);
            return;
        }

        void DestroyProjectile()
        {
            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject,lifeAfterImpact);

        }
    }
}