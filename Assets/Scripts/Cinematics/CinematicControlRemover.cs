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
            playableDirector.stopped += EnableControl;
            playableDirector.played += DisableControl;
        }
        private GameObject GetPlayer()
        {
            if (this.player == null)
                this.player = GameObject.FindWithTag("Player");
            return this.player;
        }
        void DisableControl(PlayableDirector pd)
        {

            GetPlayer().GetComponent<ActionScheduler>().CancelCurrentAction();
            GetPlayer().GetComponent<PlayerController>().enabled = false;
        }
		void EnableControl(PlayableDirector pd)
        {
            GetPlayer().GetComponent<PlayerController>().enabled = true;
        }
	}
}
