using BlackCat.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace BlackCat.Control
{
    public class ControlManager : MonoBehaviour
    {
        GameObject player;
        NPCInRange npcInRange;
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

        public void DisablePlayerControl()
        {

            GetPlayer().GetComponent<ActionScheduler>().CancelCurrentAction();
            GetPlayer().GetComponent<PlayerController>().enabled = false;
        }
        public void EnablePlayerControl()
        {
            GetPlayer().GetComponent<PlayerController>().enabled = true;
        }

        public void DisableAllControls(PlayableDirector pd)
        {
            DisablePlayerControl();
            var teste = npcInRange.GetNPCInRange();
            foreach (GameObject npc in teste)
            {
                npc.GetComponent<ActionScheduler>().CancelCurrentAction();

                var IAController = npc.gameObject.GetComponent<AIController>();
                if (IAController != null)
                {
                    IAController.enabled = false;
                }
            }
        }
        public void EnableAllControls(PlayableDirector pd)
        {
            EnablePlayerControl();
            var teste = npcInRange.GetNPCInRange();
            foreach (GameObject npc in teste)
            {
                var AIController = npc.gameObject.GetComponent<AIController>();
                
                if (AIController != null)
                {
                    AIController.enabled = true;
                }
            }
        }
    }
}
