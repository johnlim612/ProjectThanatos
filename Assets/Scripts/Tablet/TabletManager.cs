using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    private string _questLog;
    private string _diaryEntry;

    private void Awake() {
        _questLog = "";
        _diaryEntry = "";

        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.TabletKey, 1); // change to current day instead
    }

    public void Refresh() {
        _diaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        int index = 1;
        string log = "";

        foreach (string str in DialogueDataManager.Instance.GetQuestLog()) {
            log += $"{index++}: {str}\n";
        }

        _questLog = log;
    }

    public string QuestLog { 
        get { return _questLog; }
    }

    public string DiaryEntry {
        get { return _diaryEntry; }
    }
}
