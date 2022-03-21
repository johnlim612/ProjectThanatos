using UnityEngine;

public class TabletManager : MonoBehaviour {
    [SerializeField] private GameObject _tabletGameObj;
    private string _questLog;
    private string _diaryEntry;
    private GameObject _player;

    private void Start() {
        _questLog = "";
        _diaryEntry = "";
        _player = GameObject.Find(Constants.PlayerKey);
        _tabletGameObj.SetActive(false);
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.TabletKey, 1); // change to current day instead
        Refresh();
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

    /// <summary>
    /// Changes the state of the Tablet to Open or Closed based on the value of param.
    /// </summary>
    /// <param name="isOpen">If true/false, explicitly set tablet set. If null, toggle.</param>
    public void ToggleTabletState(bool? isOpen = null) {
        if (isOpen == null) {
            if (UI.UIDialogueManager.IsInteracting) {
                return;
            } else {
                print("isOpen is null && UIMgr is inactive");
                _tabletGameObj.SetActive(!_tabletGameObj.activeSelf);
                Cursor.lockState = (_tabletGameObj.activeSelf) ? CursorLockMode.None :
                                       CursorLockMode.Locked;
                return;
            }
        }
        print("isOpen: " + isOpen);
        _tabletGameObj.SetActive((bool)isOpen);
        Cursor.lockState = (bool)isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public Vector3 PlayerPosition {
        get { return _player.transform.position; }
    }

    public string QuestLog { 
        get { return _questLog; }
    }

    public string DiaryEntry {
        get { return _diaryEntry; }
    }
}
