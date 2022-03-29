using System;
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
    [SerializeField] private TabletButton _questButton;
    [SerializeField] private TabletButton _diaryButton;
    [SerializeField] private TabletButton _mapButton;
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private float _tabTransitionTime;
    [SerializeField] private GameObject _scrollView;

    private GameObject _player;
    private Color _screenTextBaseColor;
    private Color _parentBaseColor;
    private Image _parentImage;
    private AudioSource _audioSource;
    private bool _isMapOpened;
    private RectTransform _mapPolkaLolkaDotRect;
    private Image _mapPolkaLolkaDotImg;
    private Image _mapBackground;
    private bool _isUpdating; // if the update diary coroutine is running

    private const float _sentenceSpeed = 0.01f;

    private void Awake() {
        _player = GameObject.Find(Constants.PlayerKey);
        _audioSource = GetComponent<AudioSource>();
        _isUpdating = false;

        // Tablet Layout
        _parentImage = _content.transform.parent.GetComponent<Image>();
        _parentBaseColor = _parentImage.color;
        _screenTextBaseColor = _content.color;
        TabletButton.SetSelectedColorBlock(_questButton.Button.colors);
        TabletButton.SetUnselectedColorBlock(_diaryButton.Button.colors);

        // Tablet Map
        GameObject tempPolkaLolkaDot = GameObject.Find("Screen/Scroll View/PlayerMarker");
        _isMapOpened = false;
        _mapPolkaLolkaDotRect = tempPolkaLolkaDot.GetComponent<RectTransform>();
        _mapPolkaLolkaDotImg = tempPolkaLolkaDot.GetComponent<Image>();
        _mapBackground = _scrollView.GetComponent<Image>();
        _mapPolkaLolkaDotImg.enabled = false;

        // When the tablet is first opened
        _questButton.Select();
        OpenQuestTab();
    }

    private void Update() {
        if (_isMapOpened) {
            // player z position is the x position on the map
            float x = (_player.transform.position.z  + 85f) * Constants.MapXRatio - 280;
            // player x position is the y position on the map
            float y = (_player.transform.position.x - 5) * Constants.MapYRatio * -1;
            _mapPolkaLolkaDotRect.anchoredPosition = new Vector2(x, y);
        }
    }

    /// <summary>
    /// Primarily for Button's OnClick() in Unity's Inspector
    /// </summary>
    /// <param name="btn">Tablet Button game object</param>
    public void SelectTab(TabletButton btn) {
        if (btn.IsSelected() || _isUpdating) {
            return;
        }

        btn.Select();
        StartCoroutine(SwitchTabCoroutine(btn));

        switch (btn.Type) {
            case ButtonType.Quest:
                OpenQuestTab();
                break;
            case ButtonType.Diary:
                OpenDiaryTab();
                break;
            case ButtonType.Map:
                OpenMapTab();
                break;
            default:
                throw new ArgumentException("Button Type '" + btn.Type + "' not found.\nPlease " +
                    "check the 'Tablet Button' scripts and the 'Button' component's On Click() " +
                    "functions in the inspector. If this is an emergency, please call 911.");
        }
    }

    /// <summary>
    /// Update diary at the end of the day
    /// </summary>
    public IEnumerator UpdateDiaryCoroutine() {
        _isUpdating = true;
        _mapPolkaLolkaDotImg.enabled = false;

        gameObject.SetActive(true);
        _diaryButton.Select();
        OpenDiaryTab();

        string entry = "DAY " + GameManager.Instance.Day + ":\n" + TabletManager.Instance.CurrentDiaryEntry;
        _content.text = TabletManager.Instance.DiaryEntryHistory;

        for (int i = 0; i < entry.Length; i++) {
            _content.text = _content.text.Insert(i, entry[i].ToString());
            yield return new WaitForSeconds(_sentenceSpeed);
        }

        entry = entry.Insert(0, "\n\n");
        TabletManager.Instance.StoreDiaryEntry(entry);
        _isUpdating = false;
    }

    /// <summary>
    /// Animation for switching tabs
    /// </summary>
    IEnumerator SwitchTabCoroutine(TabletButton btn) {
        _parentImage.color = Color.black;
        _content.color = Color.black;

        _audioSource.Play();

        if (btn.Type == ButtonType.Map) {
            _isMapOpened = true;
        } else {
            _isMapOpened = false;
            _mapPolkaLolkaDotImg.enabled = false;
        }

        yield return new WaitForSeconds(_tabTransitionTime);

        _content.color = _screenTextBaseColor;

        if (btn.Type == ButtonType.Map) {
            _parentImage.color = Color.white;
            _mapPolkaLolkaDotImg.enabled = true;
        } else {
            _parentImage.color = _parentBaseColor;
        }
    }

    private void OpenQuestTab() {
        _mapBackground.enabled = false;
        _content.text = TabletManager.Instance.QuestLog;
    }

    private void OpenDiaryTab() {
        _mapBackground.enabled = false;
        _content.text = TabletManager.Instance.DiaryEntryHistory;
    }

    private void OpenMapTab() {
        _mapBackground.enabled = true;
        _content.text = "";
    }

    private void OnValidate() {
        if (_tabTransitionTime < 0) {
            _tabTransitionTime = 0;
        }
    }
}
