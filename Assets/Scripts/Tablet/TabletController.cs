using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// First button in Tab Buttons array should represent what a selected 
/// button looks like (it will also be automatically selected).
/// Second button should represent what a non-selected button looks like.
/// Colors of any other buttons will be changed to these colors.
/// </summary>
public class TabletController : MonoBehaviour {
    [SerializeField] private Button[] _tabButtons;
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private TabletManager _tabletManager;
    [SerializeField] private float _tabTransitionTime;
    [SerializeField] private Sprite _mapImage;

    private Button _selectedButton;
    private ColorBlock _baseColorBlock;
    private ColorBlock _selectedColorBlock;
    private Color _screenTextBaseColor;
    private Color _parentBaseColor;
    private Image _parentImage;
    private Sprite _baseMapImage;
    private AudioSource _audioSource;
    private bool _isMapOpened;

    void Start() {
        if (_tabButtons.Length < 2) {
            Debug.LogError("This controller needs at least 2 button objects for reference. The " +
                "first should be what the button looks like when selected and the second when " +
                "it isn't selected. Colors of any other buttons will be changed to these colors.");
        }

        _selectedButton = _tabButtons[0];
        _selectedColorBlock = _tabButtons[0].colors;
        _baseColorBlock = _tabButtons[1].colors;
        _parentImage = _screenText.transform.parent.GetComponent<Image>();
        _parentBaseColor = _parentImage.color;
        _screenTextBaseColor = _screenText.color;
        _baseMapImage = _parentImage.sprite;
        _audioSource = GetComponent<AudioSource>();
        _isMapOpened = false;

        OpenQuestTab();
    }

    /// <summary>
    /// Maintains color after being selected
    /// </summary>
    public void SelectTab(Button btn) {
        if (_selectedButton == btn) {
            return;
        }

        StartCoroutine(SwitchTabCoroutine(btn));
        _selectedButton.colors = _baseColorBlock;
        _selectedButton = btn;
        btn.colors = _selectedColorBlock;
    }

    /// <summary>
    /// Animation for switching tabs
    /// </summary>
    IEnumerator SwitchTabCoroutine(Button btn) {
        _parentImage.color = Color.black;
        _screenText.color = Color.black;

        _audioSource.Play();
        yield return new WaitForSeconds(_tabTransitionTime);

        _screenText.color = _screenTextBaseColor;

        if (btn.gameObject.CompareTag("TabImage")) {
            _parentImage.color = Color.white;
            _isMapOpened = true;
        } else {
            _parentImage.color = _parentBaseColor;
            _isMapOpened = false;
        }
    }

    public void OpenQuestTab() {
        _parentImage.sprite = _baseMapImage;
        _screenText.text = _tabletManager.QuestLog;
    }

    public void OpenDiaryTab() {
        _parentImage.sprite = _baseMapImage;
        _screenText.text = _tabletManager.DiaryEntry;
    }

    public void OpenMapTab() {
        _parentImage.sprite = _mapImage;
        _screenText.text = "";
    }

    public bool IsMapOpened { 
        get { return _isMapOpened; }
    }

    private void OnValidate() {
        if (_tabTransitionTime < 0) {
            _tabTransitionTime = 0;
        }
    }
}