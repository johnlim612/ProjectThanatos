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
    [SerializeField] private Sprite _mapImage;

    private GameObject _player;
    private Button _selectedButton;
    private ColorBlock _baseColorBlock;
    private ColorBlock _selectedColorBlock;
    private Color _screenTextBaseColor;
    private Color _parentBaseColor;
    private Image _parentImage;
    private Sprite _baseMapImage;
    private AudioSource _audioSource;
    private bool _isMapOpened;
    private RectTransform _mapPolkaLolkaDotRect;
    private Image _mapPolkaLolkaDotImg;

    private const float _sentenceSpeed = 0.02f;

    private void Awake() {
        _player = GameObject.Find(Constants.PlayerKey);
        _audioSource = GetComponent<AudioSource>();

        // Tablet Layout
        _parentImage = _content.transform.parent.GetComponent<Image>();
        _parentBaseColor = _parentImage.color;
        _baseMapImage = _parentImage.sprite;
        _selectedButton = _tabButtons[0];
        _selectedColorBlock = _tabButtons[0].colors;
        _baseColorBlock = _tabButtons[1].colors;
        _screenTextBaseColor = _content.color;

        // Tablet Map
        GameObject tempPolkaLolkaDot = GameObject.Find("Screen/Image");
        _isMapOpened = false;
        _mapPolkaLolkaDotRect = tempPolkaLolkaDot.GetComponent<RectTransform>();
        _mapPolkaLolkaDotImg = tempPolkaLolkaDot.GetComponent<Image>();
        _mapPolkaLolkaDotImg.enabled = false;

        OpenQuestTab();
    }

    private void Update() {
        if (_isMapOpened) {
            // player z position is the x position on the map
            float x = (_player.transform.position.z  + 89.5f) * Constants.MapXRatio - 295;
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

        string entry = "DAY " + GameManager.Instance.Day + ": " + TabletManager.Instance.CurrentDiaryEntry;
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
        _parentImage.sprite = _baseMapImage;
        _content.text = TabletManager.Instance.QuestLog;
        print("quest");
    }

    public void OpenDiaryTab() {
        if (_baseMapImage == null) {
            print("base map null");
        }

        if (_parentImage == null) {
            print("parent image null");
        }

        if (_parentImage.sprite == null) {
            print("sprite null");
        }
        _parentImage.sprite = _baseMapImage;
        _content.text = TabletManager.Instance.DiaryEntryHistory;
        print("diary");
    }

    public void OpenMapTab() {
        _parentImage.sprite = _mapImage;
        _content.text = "";
        print("map");
    }

    private void OnValidate() {
        if (_tabTransitionTime < 0) {
            _tabTransitionTime = 0;
        }
    }
}
