using System;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/New Quest")]
    public class Quest : ScriptableObject
    {
        [Serializable]
        private class Reward
        {
            public int Quantity;
            public InventoryItem Item;
        }
        
        [Serializable]
        public class Objective
        {
            public string Reference;
            public string Description;
        }
        
        [SerializeField] private List<Objective> _objectives = new List<Objective>();
        [SerializeField] private List<Reward> _rewards = new List<Reward>();

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return _objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return _objectives;
        }

        public bool HasObjective(string objectiveReference)
        {
            foreach (var objective in _objectives)
            {
                if (objective.Reference == objectiveReference)
                {
                    return true;
                }
            }
            
            return false;
        }

        public static Quest GetByName(string questName)
        {
            var quests = Resources.LoadAll<Quest>(string.Empty);
            foreach (var quest in quests)
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }

            return null;
        }
    }
}