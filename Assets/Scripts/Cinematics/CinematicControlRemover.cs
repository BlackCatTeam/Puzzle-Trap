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
        ControlManager disableControl;
        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
            disableControl = FindObjectOfType<ControlManager>();
 
        }
        private GameObject GetPlayer()
        {
            if (this.player == null)
                this.player = GameObject.FindWithTag("Player");
            return this.player;
        }
        private void OnDisable()
        {
            playableDirector.stopped -= disableControl.EnableAllControls;
            playableDirector.played  -= disableControl.DisableAllControls;
        }
        private void OnEnable()
        {
            playableDirector.stopped += disableControl.EnableAllControls;
            playableDirector.played += disableControl.DisableAllControls;
        }
	}
}
