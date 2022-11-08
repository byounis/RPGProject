using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [Serializable]
    public class QuestStatus
    {
        [SerializeField] private Quest _quest;
        [SerializeField] private List<string> _completedObjectives;

        public Quest GetQuest()
        {
            return _quest;
        }

        public int GetCompletedObjectivesCount()
        {
            return _completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return _completedObjectives.Contains(objective);
        }
    }
}