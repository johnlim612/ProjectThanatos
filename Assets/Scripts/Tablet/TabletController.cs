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
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private float _tabTransitionTime;
    [SerializeField] private GameObject _scrollView;

    private GameObject _player;
    private Button _selectedButton;
    private ColorBlock _baseColorBlock;
    private ColorBlock _selectedColorBlock;
    private Color _screenTextBaseColor;
    private Color _parentBaseColor;
    private Image _parentImage;
    private AudioSource _audioSource;
    private bool _isMapOpened;
    private RectTransform _mapPolkaLolkaDotRect;
    private Image _mapPolkaLolkaDotImg;
    private Image _mapBackground;

    private const float _sentenceSpeed = 0.01f;

    private void Awake() {
        _player = GameObject.Find(Constants.PlayerKey);
        _audioSource = GetComponent<AudioSource>();

        // Tablet Layout
        _parentImage = _content.transform.parent.GetComponent<Image>();
        _parentBaseColor = _parentImage.color;
        _selectedButton = _tabButtons[0];
        _selectedColorBlock = _tabButtons[0].colors;
        _baseColorBlock = _tabButtons[1].colors;
        _screenTextBaseColor = _content.color;

        // Tablet Map
        GameObject tempPolkaLolkaDot = GameObject.Find("Screen/Scroll View/Image");
        _isMapOpened = false;
        _mapPolkaLolkaDotRect = tempPolkaLolkaDot.GetComponent<RectTransform>();
        _mapPolkaLolkaDotImg = tempPolkaLolkaDot.GetComponent<Image>();
        _mapBackground = _scrollView.GetComponent<Image>();
        _mapPolkaLolkaDotImg.enabled = false;

        OpenQuestTab();
    }

    private void Update() {
        if (_isMapOpened) {
            // player z position is the x position on the map
            float x = (_player.transform.position.z  + 85f) * Constants.MapXRatio - 295;
            // player x position is the y position on the map
            float y = (_player.transform.position.x - 5) * Constants.MapYRatio * -1;
            _mapPolkaLolkaDotRect.anchoredPosition = new Vector2(x, y);
        }
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
    /// Update diary at the end of the day
    /// </summary>
    public IEnumerator UpdateDiary() {
        gameObject.SetActive(true);
        SelectTab(_tabButtons[1]);
        OpenDiaryTab();

        string entry = "DAY " + GameManager.Instance.Day + ":\n" + TabletManager.Instance.CurrentDiaryEntry;
        _content.text = TabletManager.Instance.DiaryEntryHistory;

        foreach (char letter in entry) {
            _content.text += letter;
            yield return new WaitForSeconds(_sentenceSpeed);
        }

        TabletManager.Instance.StoreDiaryEntry(entry);
    }

    /// <summary>
    /// Animation for switching tabs
    /// </summary>
    IEnumerator SwitchTabCoroutine(Button btn) {
        _parentImage.color = Color.black;
        _content.color = Color.black;

        _audioSource.Play();

        if (btn.gameObject.CompareTag("TabImage")) {
            _isMapOpened = true;
        } else {
            _isMapOpened = false;
            _mapPolkaLolkaDotImg.enabled = false;
        }

        yield return new WaitForSeconds(_tabTransitionTime);

        _content.color = _screenTextBaseColor;

        if (btn.gameObject.CompareTag("TabImage")) {
            _parentImage.color = Color.white;
            _mapPolkaLolkaDotImg.enabled = true;
        } else {
            _parentImage.color = _parentBaseColor;
        }
    }

    public void OpenQuestTab() {
        _mapBackground.enabled = false;
        _content.text = TabletManager.Instance.QuestLog;
    }

    public void OpenDiaryTab() {
        _mapBackground.enabled = false;
        _content.text = TabletManager.Instance.DiaryEntryHistory;
    }

    public void OpenMapTab() {
        _mapBackground.enabled = true;
        _content.text = "";
    }

    private void OnValidate() {
        if (_tabTransitionTime < 0) {
            _tabTransitionTime = 0;
        }
    }
}
