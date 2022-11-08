using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _progress;

        private Quest _quest;
        
        public void Setup(Quest quest)
        {
            _quest = quest;
            _title.SetText(quest.GetTitle());
            _progress.SetText($"0/{quest.GetObjectiveCount()}");
        }

        public Quest GetQuest()
        {
            return _quest;
        }
    }
}