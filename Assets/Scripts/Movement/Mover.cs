using BlackCat.Core;
using BlackCat.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;
namespace BlackCat.Movement
{
    public class Mover : MonoBehaviour , IAction
    {
        private NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
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
