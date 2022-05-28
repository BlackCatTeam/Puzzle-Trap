using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlackCat.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] ActionType action;
        [SerializeField] UnityEvent onTrigger;


        public void Trigger(ActionType actionType)
        {
            if (actionType == action)
            {
                onTrigger.Invoke();
            }
        }
    }
}
