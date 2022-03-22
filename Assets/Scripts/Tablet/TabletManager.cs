using UnityEngine;

public class TabletManager : MonoBehaviour {
    [SerializeField] private GameObject _tabletGameObj;
    private string _questLog;
    private string _currentDiaryEntry;
    private string _diaryEntryHistory;
    private GameObject _player;

    private void Start() {
        _questLog = "";
        _currentDiaryEntry = "";
        _diaryEntryHistory = "";
        _player = GameObject.Find(Constants.PlayerKey);
        _tabletGameObj.SetActive(false);
        Refresh();
    }

    /// <summary>
    /// Updates data in the tablet
    /// </summary>
    public void Refresh() {
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.DiaryKey);
        _currentDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        int index = 1;
        string log = "";

        DialogueDataManager.Instance.Initialize(UI.EntityType.QuestLog,
            Constants.QuestLogKey);
        foreach (string str in DialogueDataManager.Instance.GetQuestLog()) {
            log += $"{index++}: {str}\n";
        }

        _questLog = log;
    }

    /// <summary>
    /// Changes the state of the Tablet to Open or Closed based on the value of param.
    /// </summary>
    /// <param name="isOpen">If true/false, explicitly set tablet set. If null, toggle.</param>
    public void ToggleTabletState(bool? isOpen = null) {
        if (isOpen == null) {
            if (UI.UIDialogueManager.Instance.IsInteracting) {
                return;
            } else {
                _tabletGameObj.SetActive(!_tabletGameObj.activeSelf);
                Cursor.lockState = (_tabletGameObj.activeSelf) ? CursorLockMode.None :
                                       CursorLockMode.Locked;
                return;
            }
        }

        _tabletGameObj.SetActive((bool)isOpen);
        Cursor.lockState = (bool)isOpen ? CursorLockMode.None : CursorLockMode.Locked;
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
