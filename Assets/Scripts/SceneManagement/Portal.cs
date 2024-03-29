using BlackCat.Control;
using BlackCat.Core;
using BlackCat.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace BlackCat.SceneManagement {
	public class Portal : MonoBehaviour
	{
        ControlManager controlManager;

        private void Start()
        {
            controlManager = FindObjectOfType<ControlManager>(); 
        }

        [Serializable]
        enum Scenes
        {
            SandBoxA,
            SandBoxB
        };
       [Serializable]
       enum Portals
        {
            PortalCity_Florest,
            PortalCity_Lagoon,
            PortalC,
            PortalD,
        }
        [SerializeField]
        Scenes Scene = Scenes.SandBoxA;
     
        [SerializeField]
        Portals PortalIdentifier = Portals.PortalCity_Florest;
        [SerializeField]
        Transform SpawnPoint;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float WaitFadeTime = 1f;

        private void OnTriggerEnter(Collider other)
        {
          if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }  
        }


        private IEnumerator Transition() {
            DontDestroyOnLoad(this.gameObject);

            Fader fader = FindObjectOfType<Fader>();
            controlManager.DisablePlayerControl();
            yield return fader.FadeOut(fadeOutTime);
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync((int)Scene);
            controlManager.DisablePlayerControl();

            savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();

            yield return new WaitForSeconds(WaitFadeTime);
            
            fader.FadeIn(fadeInTime);
            controlManager.EnablePlayerControl();
            Destroy(this.gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach ( Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;

                if (portal.PortalIdentifier != this.PortalIdentifier) continue;               

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position);
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;


        }
    }
}
