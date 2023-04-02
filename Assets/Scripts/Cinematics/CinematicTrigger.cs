using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BlackCat.Cinematics {
	public class CinematicTrigger : MonoBehaviour
	{
        bool alreadyTrigger = false;
        private void OnTriggerEnter(Collider other)
        {

            if (!alreadyTrigger && other.gameObject.tag == "Player")
            {

                GetComponent<PlayableDirector>().Play();
                alreadyTrigger = true;

            }
        }

    }
}
