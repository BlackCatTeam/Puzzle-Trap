using BlackCat.Attributes;
using BlackCat.Core;
using BlackCat.Core.Interfaces;
using BlackCat.Saving;
using UnityEngine;
using UnityEngine.AI;
namespace BlackCat.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent navMeshAgent;
        Health health;
        [SerializeField]
        float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 1f;
        void Awake()
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
           // navMeshAgent.isStopped = true;
           navMeshAgent.enabled = false;

        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.MoveTo(destination, speedFraction);
        }
        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool HasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!HasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.enabled = true;

            navMeshAgent.destination = destination;
            navMeshAgent.speed = Mathf.Clamp01(speedFraction) * maxSpeed;
            navMeshAgent.isStopped = false;

        }

        public void Cancel()
        {
            Stop();
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            this.transform.position = ((SerializableVector3)state).ToVector3();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
