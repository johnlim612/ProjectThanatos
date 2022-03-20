using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI _screenText;

    protected List<string> _questLog = new List<string>();
    protected string _storedDiaryEntry = "";

    public void Refresh() {
        DataManager.Instance.Initialize(UI.EntityType.Diary, 
            Constants.TabletKey, GameManager.SabotageId);

        _storedDiaryEntry = DataManager.Instance.GetDiaryEntry();
        _questLog = DataManager.Instance.GetQuestLog();
    }

    public void SetVariables() {

    }

    public void OpenDiaryTab() {
        _screenText.text = _storedDiaryEntry;
    }

    public void OpenQuestTab() {
        int index = 1;
        string log = "";

        foreach (string str in _questLog) {
            log += $"{index++}: {str} ";
        }
        _screenText.text = log;
    }
}
