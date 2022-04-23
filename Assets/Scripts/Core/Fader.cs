using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core {
	public class Fader : MonoBehaviour
	{

        const int FADE_OUT_TARGET = 1;
        const int FADE_IN_TARGET = 0;
        CanvasGroup canvasGroup;
        [SerializeField]
        public float Timer = 3f;

        Coroutine currentInstanceFade;

        private void Awake()
        {
            canvasGroup = this.GetComponent<CanvasGroup>();            
        }
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        IEnumerator FadeOutIn(float time)
        {
            yield return FadeOut(time);
            print("Faded Out");
            yield return FadeIn(time);
            print("Faded In");
        }
       public Coroutine FadeOut(float time)
        {
           return Fade(FADE_OUT_TARGET, time);
        }
        public Coroutine FadeIn(float time)
        {
           return Fade(FADE_IN_TARGET,time);
        }
        public Coroutine Fade(float target, float time)
        {
            if (currentInstanceFade != null)
                StopCoroutine(currentInstanceFade);
            currentInstanceFade = StartCoroutine(FadeRoutine(target, time));
            return currentInstanceFade;
        }
        private IEnumerator FadeRoutine(float target ,float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha ,target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
