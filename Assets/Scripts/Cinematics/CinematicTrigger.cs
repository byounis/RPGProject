using System;
using GameDevTV.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField] private PlayableDirector _playableDirector;
        
        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.CompareTag("Player"))
            {
                _playableDirector.Play();
                _triggered = true;
            }
        }
        
        #region Saving
        
        public object CaptureState()
        {
            return _triggered;
        }

        public void RestoreState(object state)
        {
            _triggered = (bool) state;
        }

        #endregion
    }
}
