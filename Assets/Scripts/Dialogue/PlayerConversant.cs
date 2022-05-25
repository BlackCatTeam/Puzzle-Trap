using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackCat.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue = null;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        public event Action onConversationUpdated;
        private AIConversant aiConversant;

        public void StartDialog(Dialogue newDialogue,AIConversant newAiConversant)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            aiConversant = newAiConversant;
            onConversationUpdated();

        }
        public bool IsActive()
        {
            return currentDialogue != null;
        }
        public string GetText()
        {
            if (currentNode == null) return string.Empty;

            return currentNode.GetText();
        }
        public bool IsChoosing() => isChoosing;

        public void SelectChoice(DialogueNode chosenNode)
        {
            if (chosenNode == null) return;
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }
        public bool IsSkippable()
        {
            return currentDialogue.GetIsSkippable();
        }
        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);                     
        }

        public bool IsLastNode() => currentDialogue.IsLastNode(currentNode);
        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();

                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
            VerifyBeginAction();
            VerifyAnimation();
            VerifyDubbing();
            onConversationUpdated();

        }

        private void VerifyDubbing()
        {
            if (currentNode.GetDubbing() == null) return;
            GameObject speaker = aiConversant.GetSpeaker(currentNode.GetSpeaker());
            if (speaker == null) return;

            
            GameObject DubbingPrefab = Instantiate(currentNode.GetDubbing(), speaker.transform);
            if (DubbingPrefab == null) return;
            if (DubbingPrefab.GetComponent<AudioSource>() == null) return;

            DubbingPrefab.GetComponent<AudioSource>().Play();

            Destroy(DubbingPrefab,5f);

        }

        private void VerifyAnimation()
        {


            if (currentNode.GetAnimationClip() == null) return;

            GameObject speaker = aiConversant.GetSpeaker(currentNode.GetSpeaker());

            if (speaker.GetComponent<Animator>() == null) return;

            speaker.GetComponent<Animator>().Play("Attack");
            
        }

        private void VerifyBeginAction()
        {

            switch (currentNode.GetAction(true))
            {
                case ActionType.Nothing: return;
                case ActionType.Shop: return;
                case ActionType.Fight: return;
                default: return;
            }

        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }
        public bool HasNext() => currentDialogue.GetAllChildren(currentNode).Count() > 0;
        
    }
}