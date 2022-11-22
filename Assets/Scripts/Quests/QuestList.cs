using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Core;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
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

            if (status.IsComplete())
            {
                GiveReward(quest);
            }
            
            OnUpdatedQuestList?.Invoke();
        }
        
        public IEnumerable<QuestStatus> GetStatuses()
        {
            return _statuses;
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
        private void GiveReward(Quest quest)
        {
            var inventory = GetComponent<Inventory>();
            foreach (var reward in quest.GetRewards())
            {
                var addedToInventory = inventory.AddToFirstEmptySlot(reward.Item, reward.Quantity);
                if (!addedToInventory)
                {
                    GetComponent<ItemDropper>().DropItem(reward.Item, reward.Quantity);
                }
            }
        }
        
        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate != "HasQuest")
            {
                return null;
            }

            return HasQuest(Quest.GetByName(parameters[0]));
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (var status in _statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateList = state as List<object>;
            if (stateList == null)
            {
                return;
            }
            
            _statuses.Clear();
            foreach (var objectState in stateList)
            {
                _statuses.Add(new QuestStatus(objectState));
            }
            
            OnUpdatedQuestList?.Invoke();
        }

    }
}