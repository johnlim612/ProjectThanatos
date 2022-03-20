using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// First button in Tab Buttons array should represent what a selected 
/// button looks like (it will also be automatically selected).
/// Second button should represent what a non-selected button looks like.
/// </summary>
public class TabletController : MonoBehaviour {
    [SerializeField] private Button[] _tabButtons;
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private TabletManager _tabletManager;

    private Button _selectedBtn;
    private ColorBlock _baseColorBlock;
    private ColorBlock _selectedColorBlock;

    void Start() {
        if (_tabButtons.Length < 2) {
            Debug.LogError("This controller needs at least 2 button objects for reference. " +
                "The first should be what the button looks like when selected and the second " +
                "when it isn't selected.");
        }

        _selectedBtn = _tabButtons[0];
        _selectedColorBlock = _tabButtons[0].colors;
        _baseColorBlock = _tabButtons[1].colors;

        _tabButtons[0].onClick.Invoke();
    }

    /// <summary>
    /// Maintains color after being selected
    /// </summary>
    /// <param name="btn"></param>
    public void SetButtonColors(Button btn) {
        _selectedBtn.colors = _baseColorBlock;
        _selectedBtn = btn;
        btn.colors = _selectedColorBlock;
    }

    public void OpenDiaryTab() {
         _screenText.text = _tabletManager.DiaryEntry;
    }

    public void OpenQuestTab() {
        List<string> questLog = _tabletManager.QuestLog;
        int index = 1;
        string log = "";

        foreach (string str in questLog) {
            log += $"{index++}: {str} ";
        }

        _screenText.text = log;
    }
}
