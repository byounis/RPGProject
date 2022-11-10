using System;
using RPG.Helpers;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private QuestItemUI _questItemPrefab;
        
        private QuestList _questList;

        private void Start()
        {
            _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            _questList.OnUpdatedQuestList += UpdateUI;
        }

        private void UpdateUI()
        {
            transform.DestroyChildren();
            foreach (var questStatus in _questList.GetStatuses())
            {
                var questItemUI = Instantiate(_questItemPrefab, transform);
                questItemUI.Setup(questStatus);
            }
        }
    }
}