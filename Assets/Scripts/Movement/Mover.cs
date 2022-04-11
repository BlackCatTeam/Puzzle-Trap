using BlackCat.Core;
using BlackCat.Core.Interfaces;
using BlackCat.Saving;
using UnityEngine;
using UnityEngine.AI;
namespace BlackCat.Movement
{    
    public class Mover : MonoBehaviour, IAction,ISaveable
    {
        private NavMeshAgent navMeshAgent;
        Health health;
        [SerializeField]
        float maxSpeed = 6f;
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

        private void UpdateSpeedAnimator()
        {
            Debug.Log($"{gameObject.name} is Updating Speed Animator"); this.GetComponent<Animator>().SetFloat("forwardSpeed", this.transform.InverseTransformDirection(navMeshAgent.velocity).z);
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }
        public void StartMoveAction(Vector3 destination,float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.MoveTo(destination,speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            Debug.Log($"{gameObject.name} is Moving Speed Animator");
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
            GetComponent<ActionScheduler>().CancelCurrentAction();        }
        }
    }
