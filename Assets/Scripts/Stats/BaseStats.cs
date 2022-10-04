using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField, Range(1, 99)] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression;
        [SerializeField] private GameObject _levelUpParticleEffect;
        [SerializeField] private bool _shouldUseModifiers;

        private LazyValue<int> _currentLevel;
        private Experience _experience;
        public event Action OnLevelUp;
        
        public int GetLevel() => _currentLevel.value;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            var newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + (GetPercentageModifier(stat) / 100));
        }

        private float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (_shouldUseModifiers)
            {
                return 0;
            }
            
            var total = 0f;
            foreach (var modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (_shouldUseModifiers)
            {
                return 0;
            }
            
            var total = 0f;
            foreach (var modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int CalculateLevel()
        {
            var experience = GetComponent<Experience>();

            if (experience == null)
            {
                return _startingLevel;
            }
            
            var currentXp = experience.ExperiencePoints;
            var penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, _characterClass);

            for (var level = 1; level <= penultimateLevel; level++)
            {
                var xpToLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if (xpToLevelUp > currentXp)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}