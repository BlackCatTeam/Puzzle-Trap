using BlackCat.Control;
using BlackCat.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour, IRaycastable
{
    [SerializeField] Dialogue dialogue = null;
    [SerializeField] SpeakerType speaker = SpeakerType.NPC1;
    [Space(10)]
    [Header("A Variavel Npc 1 caso Nula, pega o GameObject de Origem")]

    [Header("SPEAKERS")]
    [Space(10)]
    [SerializeField] GameObject npc1 = null;
    [SerializeField] GameObject npc2 = null;
    [SerializeField] GameObject npc3 = null;
    [SerializeField] GameObject Enemy1 = null;
    [SerializeField] GameObject Enemy2 = null;
    [SerializeField] GameObject Enemy3 = null;
    private void Awake()
    {
        if (npc1 is null)
            npc1 = this.gameObject;
    }
    public GameObject GetSpeaker(SpeakerType speakerType)
    {
        switch (speakerType)
        {
            case SpeakerType.NPC1: return npc1;
            case SpeakerType.NPC2: return npc2;
            case SpeakerType.NPC3: return npc3;
            case SpeakerType.Enemy1: return Enemy1;
            case SpeakerType.Enemy2: return Enemy2;
            case SpeakerType.Enemy3: return Enemy3;
                default: return null;
        }        
    }
    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public bool HandleRayCast(PlayerController callingController)
    {
        if (dialogue == null) return false;

        if (Input.GetMouseButtonDown(0))
        {
            callingController.GetComponent<PlayerConversant>().StartDialog(dialogue,this);
        }
        return true;
    }
}
