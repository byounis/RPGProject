using System;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;

        public event Action OnExperienceGained;

        public float ExperiencePoints => _experiencePoints;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained();
        }
        
        #region Saving
        
        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            var experienceState = (float)state;
            _experiencePoints = experienceState;
        }

        #endregion
    }
}