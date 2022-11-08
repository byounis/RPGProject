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
        
        public void Setup(QuestStatus questStatus)
        {
            _title.SetText(questStatus.GetQuest().GetTitle());
            _objectiveContainer.DestroyChildren();
            
            foreach (var objective in questStatus.GetQuest().GetObjectives())
            {
                var prefab = questStatus.IsObjectiveComplete(objective) ? _objectiveCompletedPrefab : _objectiveIncompletePrefab;
                var objectiveGameObject = Instantiate(prefab, _objectiveContainer);
                var text = objectiveGameObject.GetComponentInChildren<TextMeshProUGUI>();
                text.SetText(objective);
            }
        }
    }
}