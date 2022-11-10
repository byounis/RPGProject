using System.Collections.Generic;

namespace RPG.Quests
{
    public class QuestStatus
    {
        private readonly Quest _quest;
        private readonly List<string> _completedObjectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            _quest = quest;
        }

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

        public void CompleteObjective(string objective)
        {
            if (_quest.HasObjective(objective) && !IsObjectiveComplete(objective))
            {
                _completedObjectives.Add(objective);
            }
        }
    }
}