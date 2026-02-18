using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        //the game objects canvas group local variable
        Coroutine currentActiveFade = null;
        //sets a refrence to the current coroutine
        public void Awake ()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            //gets the canvas group component and assigns it as a local variable.
        }

        public Coroutine Fade(float target, float time)
        {
            if(currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
                //if there is a current running coroutine, stop it
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            //starts the fade out coroutine and sets it as the active coroutine
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
            //the fade routine for fading in or out
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            //while alpha less than 1
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                //sets the alpha to fade to either 0 or 1 over the passed amount of time
                yield return null;
                //every IEnum needs a return
            }
        }
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
            //calls for a coroutine to be started fading to 1
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
            //calls for a coroutine to be started fading to 0
        }
    }
}
