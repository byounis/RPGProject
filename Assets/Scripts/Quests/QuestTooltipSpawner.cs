using GameDevTV.Core.UI.Tooltips;
using RPG.UI;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var questStatus = GetComponent<QuestItemUI>().GetQuest();
            tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}