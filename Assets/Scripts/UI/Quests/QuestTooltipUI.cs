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
        [SerializeField] private GameObject _objectivePrefab;
        
        public void Setup(Quest quest)
        {
            _title.SetText(quest.GetTitle());
            _objectiveContainer.DestroyChildren();
            
            foreach (var objective in quest.GetObjectives())
            {
                var objectiveGameObject = Instantiate(_objectivePrefab, _objectiveContainer);
                var text = objectiveGameObject.GetComponentInChildren<TextMeshProUGUI>();
                text.SetText(objective);
            }
        }
    }
}