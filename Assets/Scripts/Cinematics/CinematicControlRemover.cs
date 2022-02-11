using BlackCat.Control;
using BlackCat.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BlackCat.Cinematics {
	public class CinematicControlRemover : MonoBehaviour
	{
        PlayableDirector playableDirector;
        GameObject player;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>(); 
            GameObject player = GameObject.FindWithTag("Player");
            playableDirector.stopped += EnableControl;
            playableDirector.played += DisableControl;
        }
        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
		void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
	}
}
