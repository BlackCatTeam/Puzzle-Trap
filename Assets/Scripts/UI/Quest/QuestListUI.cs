using BlackCat.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab;
    QuestList questList;
    private void Start()
    {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.OnUpdate += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        foreach (QuestStatus status in questList.GetStatus())
        {

            Instantiate(questPrefab, this.transform).Setup(status);

        }
    }
}
