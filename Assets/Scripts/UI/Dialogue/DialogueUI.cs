using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackCat.Dialogue;
using TMPro;

namespace BlackCat.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            AIText.text = playerConversant.GetText();

        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
