using BlackCat.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BlackCat.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("A distancia que o item pode cair do alvo que o dropou")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;
        const int ATTEMPTS = 30;
        [SerializeField] int NumberDrops = 2;
        public void RandomDrop()
        {
            if (dropLibrary == null) return;

            var baseStats = GetComponent<BaseStats>();
            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
                DropItem(drop.item, drop.number);
            
        }
        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return base.GetDropLocation();
        }
    }
}