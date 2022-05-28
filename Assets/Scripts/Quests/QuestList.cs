using BlackCat.Core.Interfaces;
using BlackCat.Inventories;
using BlackCat.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BlackCat.Quests
{
    public class QuestList : MonoBehaviour , ISaveable , IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        
        public event Action OnUpdate;
        public IEnumerable<QuestStatus> GetStatus()
        {
            return statuses;
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
           QuestStatus questStatus =  new QuestStatus(quest);

            statuses.Add(questStatus);
            if (OnUpdate != null)
                OnUpdate();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus questStatus = GetQuestStatus(quest);
            if (questStatus != null)
            {
                questStatus.CompleteObjective(objective);
                if (questStatus.IsComplete())
                {
                    GiveReward(quest);
                }
                if (OnUpdate != null)
                    OnUpdate();
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus questStatus in statuses)
            {
                state.Add(questStatus.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }
        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
              bool sucess =  GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if (!sucess)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item,reward.number);
                }
            }
        }
        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus questStatus in statuses)
            {
                if (questStatus.GetQuest() == quest)
                    return questStatus;
            }
            return null;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            
            switch (predicate)
            {
                case "HasQuest": return HasQuest(Quest.GetByName(parameters[0]));
                case "CompletedQuest":
                    {
                        QuestStatus status = GetQuestStatus(Quest.GetByName(parameters[0]));
                        if (status != null)
                        return status.IsComplete();
                        return false;
                    }
            }
            return null;
        }
    }
}