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
    [Header("Tablet Buttons")]
    [SerializeField] private TabletButton _questButton;
    [SerializeField] private TabletButton _diaryButton;
    [SerializeField] private TabletButton _mapButton;
    [Header("Other")]
    [SerializeField] private float _tabTransitionTime;
    [SerializeField] private float _sentenceSpeed;
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private GameObject _scrollView;

    private GameObject _player;
    private Transform _tabletButtons;
    private Transform _characterButtons;
    private Color _screenTextBaseColor;
    private Color _parentBaseColor;
    private Image _parentImage;
    private Image _mapBackground;
    private Image _mapPolkaLolkaDotImg;
    private RectTransform _mapPolkaLolkaDotRect;
    private AudioSource _audioSource;
    private bool _isMapOpened;
    private bool _isUpdating; // if the update diary coroutine is running

    private const float _mapXOffset = 90;
    private const float _mapYOffset = -30.25f;
    private const float _mapXRatio = 650 / 125.9f;
    private const float _mapYRatio = 260 / 59.9999932f * -1;

    private void Awake() {
        _player = GameObject.Find(Constants.PlayerKey);
        _tabletButtons = gameObject.transform.Find("TabletButtons");
        _characterButtons = gameObject.transform.Find("CharacterButtons");

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
            float x = (_player.transform.position.z + _mapXOffset) * _mapXRatio;
            // player x position is the y position on the map
            float y = (_player.transform.position.x + _mapYOffset) * _mapYRatio;
            _mapPolkaLolkaDotRect.anchoredPosition = new Vector2(x, y);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            EnableCharacterSelection();
        } else if (Input.GetKeyDown(KeyCode.V)) {
            DisableCharacterSelection();
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
                // Player chooses a character
                TabletManager.Instance.ChosenCharacter = btn.Type;
                DisableCharacterSelection();
                GameManager.Instance.EndDay();
                break;
        }
    }

    /// <summary>
    /// Update diary at the end of the day.
    /// Unable to switch tabs while this coroutine is active.
    /// </summary>
    public IEnumerator UpdateDiaryCoroutine() {
        TabletManager.Instance.ToggleTabletState(true);
        _diaryButton.Select();
        OpenDiaryTab();

        Cursor.lockState = CursorLockMode.None;
        _isUpdating = true;
        _mapPolkaLolkaDotImg.enabled = false;

        string entry = "DAY " + GameManager.Instance.Day + ":\n" + TabletManager.Instance.CurrentDiaryEntry;
        TabletManager.Instance.StoreDiaryEntry("\n\n");
        _content.text = TabletManager.Instance.DiaryEntryHistory;

        for (int i = 0; i < entry.Length; i++) {
            _content.text = _content.text.Insert(i, entry[i].ToString());
            yield return new WaitForSeconds(_sentenceSpeed);
        }

        if (GameManager.Instance.Day == Constants.BodyFoundDay) {
            // Make player choose a player
            EnableCharacterSelection();
        }

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

    private void EnableCharacterSelection() {
        _tabletButtons.gameObject.SetActive(false);
        _characterButtons.gameObject.SetActive(true);
    }

    private void DisableCharacterSelection() {
        _tabletButtons.gameObject.SetActive(true);
        _characterButtons.gameObject.SetActive(false);
    }

    private void OnValidate() {
        if (_tabTransitionTime < 0) {
            _tabTransitionTime = 0;
        }

        if (_sentenceSpeed < 0) {
            _sentenceSpeed = 0;
        }
    }
}
