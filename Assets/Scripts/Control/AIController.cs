using BlackCat.Combat;
using BlackCat.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Control {
	public class AIController : MonoBehaviour
	{
		[SerializeField]
		float chaseDistance = 5f;
        GameObject player;
        Health health;
        Fighter fighterScript;
        private void Start()
        {            
            player = GameObject.FindWithTag("Player");
            fighterScript = GetComponent<Fighter>();
            health = this.GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) {return; }

            if (DistanceToPlayer() < chaseDistance && fighterScript.CanAttack(player))
            {
                    fighterScript.Attack(player);
            }
            else
            {
                fighterScript.Cancel();
            }

        }
        // Call by Unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, chaseDistance);
        }
        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, this.transform.position);
        }
    }
}
