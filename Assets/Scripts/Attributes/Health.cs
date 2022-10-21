using System;
using GameDevTV.Utils;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _regenerationPercentage = 70f;
        [SerializeField] private UnityEvent<float> _takeDamage;
        [SerializeField] private UnityEvent _onDie;
        
        private LazyValue<float> _healthPoints;
        private Animator _animator;
        private static readonly int DieAnimatorHash = Animator.StringToHash("Die");
        private ActionScheduler _actionScheduler;
        private BaseStats _baseStats;

        public bool HasDied { get; private set; }
        public float HealthPoints => _healthPoints.value;
        public float MaxHealthPoints => GetComponent<BaseStats>().GetStat(Stat.Health);

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
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
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);

            if (_healthPoints.value == 0)
            {
                _onDie.Invoke();
                Die();
                //BUG: We award experience when you attack an enemy that is currently in the dying animation.
                //Which results in being rewarded twice.
                AwardExperience(instigator);
            }
            else
            {
                _takeDamage.Invoke(damage);
            }
        }

        public float GetPercentage()
        {
            return GetFraction() * 100f;
        }
        
        public float GetFraction()
        {
            return _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
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
            _healthPoints.value = (int) Mathf.Max(_healthPoints.value, regenHealthPoints);
        }
        
        public void Heal(float healthToRestore)
        {
            _healthPoints.value = Mathf.Min(_healthPoints.value + healthToRestore, MaxHealthPoints);
        }
        
        #region Saving
        
        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            var healthState = (float)state;
            _healthPoints.value = healthState;
            
            if (_healthPoints.value == 0)
            {
                Die();
            }
        }

        #endregion

        
    }
}

