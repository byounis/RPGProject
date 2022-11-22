using System;
using System.Collections.Generic;

namespace RPG.Quests
{
    public class QuestStatus
    {
        [Serializable]
        private class Record
        {
            public string QuestName;
            public List<string> CompletedObjectives;

            public Record(Quest quest, List<string> completedObjectives)
            {
                QuestName = quest.name;
                CompletedObjectives = completedObjectives;
            }
        }
        
        private readonly Quest _quest;
        private readonly List<string> _completedObjectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            _quest = quest;
        }
        
        public QuestStatus(object objectState)
        {
            var record = objectState as Record;
            if (record == null)
            {
                return;
            }

            _quest = Quest.GetByName(record.QuestName);
            _completedObjectives = record.CompletedObjectives;
        }

        public Quest GetQuest()
        {
            return _quest;
        }
        
        public bool IsComplete()
        {
            foreach (var objective in _quest.GetObjectives())
            {
                if (!_completedObjectives.Contains(objective.Reference))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetCompletedObjectivesCount()
        {
            return _completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return _completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (_quest.HasObjective(objective) && !IsObjectiveComplete(objective))
            {
                _completedObjectives.Add(objective);
            }
        }

        public object CaptureState()
        {
            return new Record(_quest, _completedObjectives);
        }
    }
}