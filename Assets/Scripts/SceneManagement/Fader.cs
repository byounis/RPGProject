using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine _currentlyActiveCoroutine;

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        private IEnumerator Fade(float target, float time)
        {
            if (_currentlyActiveCoroutine != null)
            {
                StopCoroutine(_currentlyActiveCoroutine);
            }
            
            _currentlyActiveCoroutine = StartCoroutine(FadeRoutine(target, time));
            yield return _currentlyActiveCoroutine;
        }

        public IEnumerator FadeOut(float time)
        { 
            return Fade(1, time);
        }
        
        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha,target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}