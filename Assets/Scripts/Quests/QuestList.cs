using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        private readonly List<QuestStatus> _statuses = new List<QuestStatus>();

        public event Action OnUpdatedQuestList;

        public void AddQuest(Quest quest)
        {
            var questStatus = new QuestStatus(quest);
            _statuses.Add(questStatus);

            OnUpdatedQuestList?.Invoke();
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            var status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            OnUpdatedQuestList?.Invoke();
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (var status in _statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }
        
        public IEnumerable<QuestStatus> GetStatuses()
        {
            return _statuses;
        }
    }
}