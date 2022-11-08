using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _progress;

        private QuestStatus _questStatus;
        
        public void Setup(QuestStatus questStatus)
        {
            _questStatus = questStatus;
            _title.SetText(questStatus.GetQuest().GetTitle());
            _progress.SetText($"{questStatus.GetCompletedObjectivesCount()}/{questStatus.GetQuest().GetObjectiveCount()}");
        }

        public QuestStatus GetQuest()
        {
            return _questStatus;
        }
    }
}