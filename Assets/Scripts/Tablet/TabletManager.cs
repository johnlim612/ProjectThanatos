using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletManager : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI _screenText;

    protected List<string> _questLog = new List<string>();
    protected string _storedDiaryEntry = "";

    public void Refresh() {
        print("refresh tablet");
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary, 
            Constants.TabletKey, GameManager.SabotageId);

        _storedDiaryEntry = DialogueDataManager.Instance.GetDiaryEntry();
        _questLog = DialogueDataManager.Instance.GetQuestLog();
    }

    public void OpenDiaryTab() {
        _screenText.text = DialogueDataManager.Instance.GetDiaryEntry();
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
