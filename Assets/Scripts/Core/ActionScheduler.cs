using BlackCat.Core.Interfaces;
using UnityEngine;

namespace BlackCat.Core {

    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
                print(currentAction + " is Canceled");

            }
            currentAction = action;
        }

        public IAction GetCurrentAction()
        {
            return currentAction;
        }
        }
}

