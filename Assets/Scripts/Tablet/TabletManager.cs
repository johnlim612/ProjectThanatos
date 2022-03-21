using UnityEngine;

public class TabletManager : MonoBehaviour {
    private string _questLog;
    private string _currentDiaryEntry;
    private string _diaryEntryHistory;
    private GameObject _player;

    private void Start() {
        _questLog = "";
        _currentDiaryEntry = "";
        _diaryEntryHistory = "";
        _player = GameObject.Find(Constants.PlayerKey);

        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.TabletKey, 1); // change to current day instead
        Refresh();
    }

    /// <summary>
    /// Updates data in the tablet
    /// </summary>
    public void Refresh() {
        _currentDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        int index = 1;
        string log = "";

        foreach (string str in DialogueDataManager.Instance.GetQuestLog()) {
            log += $"{index++}: {str}\n";
        }

        _questLog = log;
    }

    public void StoreDiaryEntry(string entry) {
        _diaryEntryHistory += entry + "\n\n";
    }

    public Vector3 PlayerPosition {
        get { return _player.transform.position; }
    }

    public string QuestLog { 
        get { return _questLog; }
    }

    public string CurrentDiaryEntry {
        get { return _currentDiaryEntry; }
    }

    public string DiaryEntryHistory {
        get { return _diaryEntryHistory; }
    }
}
