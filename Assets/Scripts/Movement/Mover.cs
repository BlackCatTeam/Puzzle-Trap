using BlackCat.Core;
using BlackCat.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;
namespace BlackCat.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent navMeshAgent;
        Health health;
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();           
            UpdateSpeedAnimator();
        }

        private void UpdateSpeedAnimator() => this.GetComponent<Animator>().SetFloat("forwardSpeed", this.transform.InverseTransformDirection(navMeshAgent.velocity).z);

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }
        public void MoveToAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;

        }

        public void Cancel()
        {
            Stop();
        }
    }
}
