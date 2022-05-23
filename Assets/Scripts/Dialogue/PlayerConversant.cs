using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackCat.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;

        public string GetText()
        {
            if (currentDialogue == null) return string.Empty;
            return currentDialogue.GetRootNode().GetText();
        }
    }
}