using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    //TODO: Create Property Drawer to show level and value rather than element number of the array
    [CreateAssetMenu(fileName = "New Progression", menuName = "RPG/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [Serializable]
        private class ProgressionCharacter
        {
            public CharacterClass _characterClass;
            public ProgressionStat[] _stats;
        }

        [Serializable]
        private class ProgressionStat
        {
            public Stat Stat;
            public float[] Levels;
        }
        
        [SerializeField] private ProgressionCharacter[] _progressionCharacters;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookUpTable;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            var levels = _lookUpTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            
            var levels = _lookUpTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (_lookUpTable != null)
            {
                return;
            }

            _lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var progressionCharacter in _progressionCharacters)
            {
                var statDictionary = new Dictionary<Stat, float[]>();
                foreach (var progressionStat in progressionCharacter._stats)
                {
                    statDictionary[progressionStat.Stat] = progressionStat.Levels;
                }

                _lookUpTable[progressionCharacter._characterClass] = statDictionary;
            }
        }
    }
}