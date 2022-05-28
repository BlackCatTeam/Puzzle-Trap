using BlackCat.Core.UI.Tooltips;
using BlackCat.Quests;
using BlackCat.UI.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core.UI.Quests { 
public class QuestTooltipSpawner : TooltipSpawner
{
        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus questStatus = GetComponent<QuestItemUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }

        
 
 
}
}