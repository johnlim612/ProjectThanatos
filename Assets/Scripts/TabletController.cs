using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// First button in Tab Buttons array should represent what a selected 
/// button looks like (it will also be automatically selected).
/// Second button should represent what a non-selected button looks like.
/// </summary>
public class TabletController : MonoBehaviour {
    [SerializeField] private Button[] _tabButtons;

    private Button _selectedBtn;
    private ColorBlock _baseColorBlock;
    private ColorBlock _selectedColorBlock;

    void Start() {
        if (_tabButtons.Length < 2) {
            Debug.LogError("Tab Buttons field requires at least 2 Button objects.");
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
}
