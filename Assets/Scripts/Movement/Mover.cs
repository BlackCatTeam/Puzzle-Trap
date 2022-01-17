using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace BlackCat.Movement
{
    public class Mover : MonoBehaviour
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


        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
        }
    }
}
