using GameDevTV.Core.UI.Tooltips;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}