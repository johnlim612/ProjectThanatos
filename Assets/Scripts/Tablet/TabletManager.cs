using System.Collections;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    public static TabletManager Instance { get { return _instance; } }
    public static bool TabletActive;

    [SerializeField] private GameObject _tabletGameObj;

    private static TabletManager _instance;
    private string _questLog;
    private string _currentDiaryEntry;
    private string _diaryEntryHistory;
    // super sketch I know, but pretend this is null pls n thx
    private ButtonType _chosenCharacter = ButtonType.Diary; 

    private const float _toggleSpeed = 0.00001f;
    private const float _toggleIncrement = 0.25f;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
        TabletActive = false;
    }

    private void Start() {
        _questLog = "";
        _currentDiaryEntry = "";
        _diaryEntryHistory = "";
        _tabletGameObj.SetActive(false);
        Refresh();
    }

    /// <summary>
    /// Updates data in the tablet
    /// </summary>
    public void Refresh() {
        int index = 1;

        // Update quest log
        DialogueDataManager.Instance.Initialize(UI.EntityType.QuestLog,
            Constants.QuestLogKey);
        foreach (string str in DialogueDataManager.Instance.GetQuestLog()) {
            _questLog += $"{index++}: {str}\n";
        }
        _questLog += "\n";

        // Update diary
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary,
            Constants.DiaryKey);
        _currentDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
    }

    /// <summary>
    /// Changes the state of the Tablet to Open or Closed based on the value of param.
    /// </summary>
    public void ToggleTabletState(bool state) {
        if (UI.UIDialogueManager.Instance.IsInteracting) {
            return;
        }

        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        AudioManager.Instance.Play("tabletOn");

        if (state) {
            TabletActive = true;
            FindObjectOfType<PlayerController>().ControlActive(false);
            _tabletGameObj.SetActive(state);
            StartCoroutine(OpenTabletCoroutine());
        } else {
            StartCoroutine(CloseTabletCoroutine());
        }
    }

    IEnumerator OpenTabletCoroutine() {
        float yScale = 0;
        Vector3 scale = new Vector3(1, 0, 1);
        _tabletGameObj.transform.localScale = scale;

        while (yScale < 1) {
            yScale += _toggleIncrement;
            scale.y = yScale;

            _tabletGameObj.transform.localScale = scale;
            yield return new WaitForSeconds(_toggleSpeed);
        }
    }

    IEnumerator CloseTabletCoroutine() {
        float yScale = 1;
        Vector3 scale = Vector3.one;
        _tabletGameObj.transform.localScale = scale;

        while (yScale > 0) {
            yScale -= _toggleIncrement;
            scale.y = yScale;

            _tabletGameObj.transform.localScale = scale;
            yield return new WaitForSeconds(_toggleSpeed);
        }

        _tabletGameObj.SetActive(false);
        TabletActive = false;
        FindObjectOfType<PlayerController>().ControlActive(true);
    }

    public void StoreDiaryEntry(string entry) {
        _diaryEntryHistory = _diaryEntryHistory.Insert(0, entry);
    }

    public bool IsOpened() {
        return _tabletGameObj.activeSelf;
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

    public ButtonType ChosenCharacter {
        get { return _chosenCharacter; }
        set { _chosenCharacter = value; }
    }
}
