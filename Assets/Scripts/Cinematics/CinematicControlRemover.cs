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
        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();            
 
        }
        private GameObject GetPlayer()
        {
            if (this.player == null)
                this.player = GameObject.FindWithTag("Player");
            return this.player;
        }
        private void OnDisable()
        {
            playableDirector.stopped -= EnableControl;
            playableDirector.played  -= DisableControl;
        }
        private void OnEnable()
        {
            playableDirector.stopped += EnableControl;
            playableDirector.played += DisableControl;
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
