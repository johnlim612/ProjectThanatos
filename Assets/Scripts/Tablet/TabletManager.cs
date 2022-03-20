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
            Constants.TabletKey, 1); // change to current day instead
    }

    public void Refresh() {
        QuestLog = DialogueDataManager.Instance.GetQuestLog();
        DiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
    }
}
