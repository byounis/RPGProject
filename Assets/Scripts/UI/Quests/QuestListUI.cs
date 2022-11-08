using System;
using RPG.Helpers;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private Quest[] tempQuests;
        [SerializeField] private QuestItemUI _questItemPrefab;

        private void Start()
        {
            transform.DestroyChildren();
            
            foreach (var quest in tempQuests)
            {
                var questItemUI = Instantiate(_questItemPrefab, transform);
                questItemUI.Setup(quest);
            }
        }
    }
}