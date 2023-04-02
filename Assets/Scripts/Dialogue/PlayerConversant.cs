using BlackCat.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackCat.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string PlayerName;
        Dialogue currentDialogue = null;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        public event Action onConversationUpdated;
        private AIConversant aiConversant;


        public void StartDialog(Dialogue newDialogue,AIConversant newAiConversant)
        {
            aiConversant = newAiConversant;
            currentDialogue = newDialogue;
            Debug.Log("How Many Nodes have: " + currentDialogue.GetAllNodes().Count());
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
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
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return PlayerName;
            }
            else
            {
                return aiConversant.GetSpeaker(currentNode.GetSpeaker()).GetComponent<AIConversant>().GetName();
            }
        }

        public bool IsSkippable()
        {
            return currentDialogue.GetIsSkippable();
        }
        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));                     
        }

        public bool IsLastNode() => currentDialogue.IsLastNode(currentNode);
        public void Next()
        {

            int numPlayerResponses =FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            Debug.Log("numPlayerResponses: "+ numPlayerResponses);
            Debug.Log("HasNext" + HasNext());
            if (numPlayerResponses > 0)
            {
                isChoosing = true; 
                TriggerExitAction();
                onConversationUpdated();

                return;
            }
            TriggerExitAction();
            if (HasNext())
            {
                DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
          
                currentNode = children[randomIndex];
                TriggerEnterAction();
                VerifyAnimation();
                VerifyDubbing();
                onConversationUpdated();
            }
            else
            {
                Quit();
            }
          

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


        private void TriggerEnterAction()
        {
            VerifyAction(isEnterAction: true);
        }
        private void TriggerExitAction()
        {
            VerifyAction(isEnterAction:false);
        }

        private void VerifyAction(bool isEnterAction)
        {
            if (currentNode == null) return;            
            TriggerAction(currentNode.GetAction(isEnterAction));
                    
            
        }

        private void TriggerAction(ActionType actionType)
        {
            if (actionType == ActionType.Nothing) return;

            foreach(DialogueTrigger trigger in aiConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(actionType);
            }
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            aiConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }
        public bool HasNext() => currentDialogue.GetAllChildren(currentNode).Count() > 0;

        public IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators() 
        {
            return GetComponents<IPredicateEvaluator>();
        }
    }
}