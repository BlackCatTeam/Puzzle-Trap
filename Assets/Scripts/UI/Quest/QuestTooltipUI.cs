using BlackCat.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlackCat.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewards;
        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }
            foreach (var objective in quest.GetObjectives())
            {
                GameObject prefab = status.IsObjectiveComplete(objective.reference) ? objectivePrefab : objectiveIncompletePrefab; 
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }

            rewards.text = GetRewardText(quest);      

        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = string.Empty;

            foreach( var reward in quest.GetRewards())
            {
                if (rewardText != string.Empty)
                {
                    rewardText += ", ";
                }
                if (reward.number > 1)
                    rewardText += reward.number + " ";
                rewardText += reward.item.GetDisplayName();
            }
            if (rewardText == string.Empty)
            {
                rewardText = "Sem recompensa";
            }
            rewardText += ".";
            return rewardText;
        }
    }
}