using BlackCat.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace BlackCat.Control
{
    public class ControlManager : MonoBehaviour
    {
        GameObject player;
        NPCInRange npcInRange;

        public bool IsDisable = false;
        private void Start()
        {
            npcInRange = GetPlayer().GetComponentInChildren<NPCInRange>();
        }
        private GameObject GetPlayer()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            return player;
        }

        public void DisableTargetControl(GameObject target) => DisableIA(target);

        public void DisablePlayerControl()
        {

            GetPlayer().GetComponent<ActionScheduler>().CancelCurrentAction();
            GetPlayer().GetComponent<PlayerController>().enabled = false;
            IsDisable = true;
        }
        public void EnablePlayerControl()
        {
            GetPlayer().GetComponent<PlayerController>().enabled = true;
            IsDisable = false;
        }

        public void DisableAllControls(PlayableDirector pd)
        {
            DisablePlayerControl();
            foreach (GameObject npc in npcInRange.GetNPCInRange())
            {
                DisableIA(npc);
            }
            IsDisable = true;
        }

        private void DisableIA(GameObject npc)
        {
            npc.GetComponent<ActionScheduler>().CancelCurrentAction();

            var IAController = npc.gameObject.GetComponent<AIController>();
            if (IAController != null)
            {
                IAController.enabled = false;
            }
        }
        private void EnableIA(GameObject npc)
        {
            var AIController = npc.gameObject.GetComponent<AIController>();

            if (AIController != null)
            {
                AIController.enabled = true;
            }
        }
        public void EnableAllControls(PlayableDirector pd)
        {
            EnablePlayerControl();
            foreach (GameObject npc in npcInRange.GetNPCInRange())
            {
                EnableIA(npc);
            }
            IsDisable = false;
        }

  
    }
}
