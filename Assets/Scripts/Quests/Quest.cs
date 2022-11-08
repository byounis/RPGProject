using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/New Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string[] _objectives;

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return _objectives.Length;
        }

        public IEnumerable<string> GetObjectives()
        {
            return _objectives;
        }
    }
}