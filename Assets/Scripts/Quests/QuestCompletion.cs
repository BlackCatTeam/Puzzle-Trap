using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;

     //   [Dropdown("quest.GetStringObjectives()")]
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
            

        }
    }
}