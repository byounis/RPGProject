using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private Quest _quest;

        public void GiveQuest()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            if (questList.HasQuest(_quest))
            {
                return;
            }
            
            questList.AddQuest(_quest);
        }
    }
}
