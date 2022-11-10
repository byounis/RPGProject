using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/New Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] private List<string> _objectives = new List<string>();

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return _objectives.Count;
        }

        public IEnumerable<string> GetObjectives()
        {
            return _objectives;
        }

        public bool HasObjective(string objective)
        {
            return _objectives.Contains(objective);
        }
    }
}