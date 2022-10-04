using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _regenerationPercentage = 70f;
        
        private int _healthPoints = -1;
        private Animator _animator;
        private static readonly int DieAnimatorHash = Animator.StringToHash("Die");
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;

        public bool HasDied { get; private set; }
        public float HealthPoints => _healthPoints;
        public float MaxHealthPoints => GetComponent<BaseStats>().GetStat(Stat.Health);

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            //Has not been restored and is still the uninitialized value
            if(_healthPoints < 0)
            {
                _healthPoints = (int) _baseStats.GetStat(Stat.Health);
            }
        }

        private void OnEnable()
        {
            _baseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage: " + damage);
            
            _healthPoints = Mathf.Max(_healthPoints - (int)damage, 0);

            if (_healthPoints == 0)
            {
                Die();
                //BUG: We award experience when you attack an enemy that is currently in the dying animation.
                //Which results in being rewarded twice.
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return _healthPoints * 100f / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (HasDied)
            {
                return;
            }

            HasDied = true;
            
            _animator.SetTrigger(DieAnimatorHash);
            _actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null)
            {
                return;
            }
            
            var experienceReward = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            experience.GainExperience(experienceReward);
        }

        private void RegenerateHealth()
        {
            var regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (_regenerationPercentage / 100f);
            _healthPoints = (int) Mathf.Max(_healthPoints, regenHealthPoints);
        }
        
        #region Saving
        
        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            var healthState = (int)state;
            _healthPoints = healthState;
            
            if (_healthPoints == 0)
            {
                Die();
            }
        }

        #endregion
    }
}

