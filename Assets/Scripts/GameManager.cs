using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private List<NPC> _npcList = new List<NPC>();
    [SerializeField] private TabletManager _tabletMgr;
    [SerializeField] private int _day;
    [SerializeField] private GameObject _screen;
    [SerializeField] private GameObject _roomNameWrapper;
    [SerializeField] private Fade _fade;
    [SerializeField] private List<InteractableSabotage> _sabotages;

    private static GameManager _instance;
    private List<string> _randomEventIds = new List<string>(); // Corresponds to Event-specific Dialogue Ids
    private int _sabotageId;   // The ID of the day's major event/sabotage
    private InteractableSabotage _currentSabotage;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _fade = GetComponent<Fade>();
    }

    private void Start() {
        _day = 0;   // TODO: Reset to 0 after testing.
        AdvanceDay();
    }

    public void EndDay() {
        StartCoroutine(WaitForFade());
        FindObjectOfType<LightAdjuster>().LightDecrease();
    }

    public void AdvanceDay() {
        _day++;

        CurrentSabotage = _sabotages[_day - 1];
        SabotageId = CurrentSabotage.Id;
        CurrentSabotage.ToggleActiveState();
        LightAdjuster.SabotageLevel = LightAdjuster.LightType.EMERGENCY;

        _tabletMgr.Refresh();
        UI.UIDialogueManager.Instance.InitializeDialogue(UI.EntityType.Alert);

        foreach (NPC npc in _npcList) {
            npc.HasBeenSpokenTo = false;
        }
    }

    public void ToggleRoomName(bool isRoomEntered, string enteredRoomName) {
        Animator anim = _roomNameWrapper.GetComponent<Animator>();
        Transform roomName = _roomNameWrapper.transform.Find("RoomName");
        TextMeshProUGUI roomNameText = roomName.GetComponent<TextMeshProUGUI>();

        roomNameText.text = enteredRoomName;
        anim.SetBool("reveal", isRoomEntered);
    }

    private IEnumerator WaitForFade() {
        _fade.FadeActive(true);
        _fade.FadeOut();
        yield return new WaitForSeconds(1);
        _fade.FadeIn();
        yield return new WaitForSeconds(1);
        _fade.FadeActive(false);
        AdvanceDay();
    }

    public int Day {
        get { return _day; }
    }

    public int SabotageId {
        get { return _sabotageId; }
        set { _sabotageId = value; }
    }

    public InteractableSabotage CurrentSabotage {
        get { return _currentSabotage; }
        private set { _currentSabotage = value;  }
    }
}
