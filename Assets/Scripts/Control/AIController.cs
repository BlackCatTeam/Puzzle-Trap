using BlackCat.Attributes;
using BlackCat.Combat;
using BlackCat.Core;
using BlackCat.Movement;
using BlackCat.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        [SerializeField]
        float shoutDistance = 5f;

        LazyValue<Vector3> guardPosition;
        [SerializeField]
        float timePauseBetweenWaypoint = 3f;
        float timeSpendInWaypoint = Mathf.Infinity;


        [SerializeField]
        float timeSuspicious = 3f;
        [SerializeField] 
        float AgroCooldown = 5f;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        [SerializeField]
        [Range(0,1)]
        float patrolSpeedFraction = 0.2f;
        [SerializeField]
        [Range(0, 1)]
        float chaseSpeedFraction = 0.4f;
        NavMeshAgent navMeshAgent;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighterScript = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
            moveScript = this.GetComponent<Mover>(); 
            navMeshAgent = this.GetComponent<NavMeshAgent>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if ((IsAggravate() && fighterScript.CanAttack(player)))
            {
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
            UpdateTimers();
        }

        private bool IsAggravate()
        {
            return DistanceToPlayer() < chaseDistance || timeSinceAggrevated < AgroCooldown;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

        }
        public void Aggravate()
        {
            timeSinceAggrevated = 0;
        }

        private Vector3 GetGuardPosition()
        {
            return this.transform.position;
        }

        private void SuspiciousBehavior()
        {
            //TODO: Fazer ir até o ultimo lugar que viu o player
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0f;
            fighterScript.Attack(player);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggravate();
            }

        }

        private void GuardBehavior()
        {
            Vector3 nextPosition = guardPosition.value;
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
                moveScript.StartMoveAction(nextPosition,patrolSpeedFraction);
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
