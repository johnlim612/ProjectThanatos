using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _screenText;

    private List<string> _questLog = new List<string>();
    private string _storedDiaryEntry = "";

    private void Awake() {
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary, 
            Constants.TabletKey, GameManager.SabotageId);

        _storedDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        _questLog = DialogueDataManager.Instance.GetQuestLog();
    }

    public void Refresh() {
        _storedDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        _questLog = DialogueDataManager.Instance.GetQuestLog();
    }

    public void OpenDiaryTab() {
        _screenText.text = DialogueDataManager.Instance.GetDiaryEntry();
    }

    public void OpenQuestTab() {
        _questLog = DialogueDataManager.Instance.GetQuestLog();
        int index = 1;
        string log = "";
        foreach (string str in _questLog) {
            log += $"{index++}: {str} ";
        }
       // _screenText.text = log;
       // print("log: " + log);
    }
}
