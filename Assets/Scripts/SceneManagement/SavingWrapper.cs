using BlackCat.Core;
using BlackCat.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime =0.2f;
        CanvasGroup canvasGroup;
        private IEnumerator Start()
        {
            Fader fade = FindObjectOfType<Fader>();
            fade.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fade.FadeIn(fadeInTime);
        }
     
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
