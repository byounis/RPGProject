using System;
using RPG.Helpers;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Transform _objectiveContainer;
        [SerializeField] private GameObject _objectiveIncompletePrefab;
        [SerializeField] private GameObject _objectiveCompletedPrefab;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        public void Setup(QuestStatus questStatus)
        {
            var quest = questStatus.GetQuest();
            _title.SetText(quest.GetTitle());
            _objectiveContainer.DestroyChildren();
            
            foreach (var objective in quest.GetObjectives())
            {
                var prefab = questStatus.IsObjectiveComplete(objective.Reference) ? _objectiveCompletedPrefab : _objectiveIncompletePrefab;
                var objectiveGameObject = Instantiate(prefab, _objectiveContainer);
                var text = objectiveGameObject.GetComponentInChildren<TextMeshProUGUI>();
                text.SetText(objective.Description);
            }

            _rewardText.SetText(GetRewardTest(quest));
        }

        private string GetRewardTest(Quest quest)
        {
            var rewardText = string.Empty;
            foreach (var reward in quest.GetRewards())
            {
                if (rewardText != string.Empty)
                {
                    rewardText += ", ";
                }

                if (reward.Quantity > 1)
                {
                    rewardText += reward.Quantity + " ";
                    
                }
                rewardText += reward.Item.GetDisplayName();
            }

            if (rewardText == string.Empty)
            {
                rewardText = "No reward";
            }
            
            return rewardText;
        }
    }
}