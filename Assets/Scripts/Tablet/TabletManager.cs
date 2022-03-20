using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    private List<string> _questLog;
    private string _diaryEntry;

    private void Awake() {
        _questLog = new List<string>();
        _diaryEntry = "";

        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.TabletKey, 1); // change to current day instead
    }

    public void Refresh() {
        _questLog = DialogueDataManager.Instance.GetQuestLog();
        _diaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
    }

    public List<string> QuestLog { 
        get { return _questLog; }
    }

    public string DiaryEntry {
        get { return _diaryEntry; }
    }
}
