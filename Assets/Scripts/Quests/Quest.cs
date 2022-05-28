using BlackCat.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Quests
{
    [CreateAssetMenu(fileName ="New Quest",menuName = "Black Cat/Quest",order = 0)]
    public class Quest : ScriptableObject
    {       
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();
        [Serializable]
        public class Objective
        {
            public string reference;
            public string description;
            public int count;
        }
        [Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }


        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }
        public IEnumerable<string> GetStringObjectives()
        {
            List<string> listStringObjectives = new List<string>();
            foreach(var objective in GetObjectives())
            {
                listStringObjectives.Add(objective.reference);
            }
            return listStringObjectives;
        }

        public string GetTitle()
        {
            return name;
        }
        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }
        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach(Objective objective in objectives)
            {
                if (objective.reference == objectiveRef)
                    return true;
            }
            return false;
        }


        public static Quest GetByName(string questName)
        {
           foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                    return quest;
            }
            return null;
        }
    }

}