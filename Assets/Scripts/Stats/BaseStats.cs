using System;
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

        private int _currentLevel = 0;
        public event Action OnLevelUp;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            var experience = GetComponent<Experience>();

            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            var newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
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

        public int GetLevel()
        {
            if (_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            
            return _currentLevel;
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