using GameDevTV.Core.UI.Tooltips;
using RPG.UI;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var quest = GetComponent<QuestItemUI>().GetQuest();
            tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}