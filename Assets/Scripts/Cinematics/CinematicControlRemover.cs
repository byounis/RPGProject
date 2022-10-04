using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _playableDirector;

        private GameObject _player;
        private ActionScheduler _actionScheduler;
        private PlayerController _playerController;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _actionScheduler = _player.GetComponent<ActionScheduler>();
            _playerController = _player.GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            _actionScheduler.CancelCurrentAction();
            _playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            _playerController.enabled = true;
        }
    }
}
