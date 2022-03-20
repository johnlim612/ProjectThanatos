using UnityEngine;

public class TabletManager : MonoBehaviour {
    private string _questLog;
    private string _diaryEntry;
    private GameObject _player;

    private void Start() {
        _questLog = "";
        _diaryEntry = "";
        _player = GameObject.Find(Constants.PlayerKey);

        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.TabletKey, 1); // change to current day instead
    }

    /// <summary>
    /// Updates data in the tablet
    /// </summary>
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
