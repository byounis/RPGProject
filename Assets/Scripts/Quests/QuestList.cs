using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private List<QuestStatus> _statuses;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return _statuses;
        }
    }
}