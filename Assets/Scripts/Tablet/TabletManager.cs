using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    public List<string> QuestLog { get; private set; }
    public string DiaryEntry { get; private set; }

    private void Start() {
        QuestLog = new List<String>();
        DiaryEntry = "";

        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary, 
            Constants.TabletKey, GameManager.SabotageId);

        print("hello");
    }

    public void Refresh() {
        print("hi");
        StartCoroutine(RefreshCoroutine());
    }

    IEnumerator RefreshCoroutine() {
        print("start");
        QuestLog = DialogueDataManager.Instance.GetQuestLog();
        DiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();

        while (QuestLog.Count == 0 || DiaryEntry == "") {
            yield return new WaitForSeconds(0.5f);
        }

        print("Quest " + QuestLog);
        print("Diary " + DiaryEntry);
    }
}
