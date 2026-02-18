using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {

        IAction currentAction;
        //sets the current action
        public void StartAction(IAction action)
        {
            if (currentAction == action) {return; }
            //if the new action is the current action, do not continue.
            if (currentAction != null)
            {
                currentAction.Cancel();
                //if the current action isnt the previous action AND isnt null, cancel the current action
            }
            currentAction = action;
            //set the current action to the new action
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
            //stops any action
        }

    }
}
