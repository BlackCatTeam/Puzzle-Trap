using BlackCat.Combat;
using BlackCat.Core;
using BlackCat.Movement;
using System;
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
        Mover moveScript;
        [SerializeField]
        PatrolPath patrolPath;
        [SerializeField]
        float waypointTolerance = 1f;
        int CurrentWaypointIndex = 0;




        Vector3 guardPosition;
        [SerializeField]
        float timePauseBetweenWaypoint = 3f;
        float timeSpendInWaypoint = Mathf.Infinity;


        [SerializeField]
        float timeSuspicious = 3f;
        float timeSinceLastSawPlayer = Mathf.Infinity;


        private void Start()
        {            
            player = GameObject.FindWithTag("Player");
            fighterScript = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
            moveScript = this.GetComponent<Mover>();
            guardPosition = this.transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) {return; }

            if (DistanceToPlayer() < chaseDistance && fighterScript.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < timeSuspicious)
            {
                SuspiciousBehavior();
            }
            else
            {
                GuardBehavior();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        } 
        private void SuspiciousBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighterScript.Attack(player);
        }

        private void GuardBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSpendInWaypoint += Time.deltaTime;
                if (timeSpendInWaypoint > timePauseBetweenWaypoint)
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (this.transform.position != nextPosition)
                moveScript.StartMoveAction(nextPosition);
        }
        public Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(CurrentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(this.transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            timeSpendInWaypoint = 0f;
         CurrentWaypointIndex =   patrolPath.GetNextPosition(CurrentWaypointIndex);
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
