using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackCat.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace BlackCat.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI SpeakerName;
            

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());
            UpdateUI();


        }

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) return;

            if (!playerConversant.IsChoosing())
                SpeakerName.text = playerConversant.GetCurrentConversantName();
            quitButton.gameObject.SetActive(playerConversant.IsSkippable());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            nextButton.gameObject.SetActive(!playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
                quitButton.gameObject.SetActive(playerConversant.IsLastNode());

            }
        }    
        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceButton = Instantiate(choicePrefab, choiceRoot);
                var textComponent = choiceButton.GetComponentInChildren<TextMeshProUGUI>();                
                textComponent.text = choice.GetText();
                var button = choiceButton.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choice);
                } 
                );

            }
        }
    }
}
