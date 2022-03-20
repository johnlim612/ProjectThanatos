using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private List<NPC> _npcList = new List<NPC>();
    [SerializeField] private TabletManager _tabletMgr;
    [SerializeField] private int _day;
    [SerializeField] private GameObject _screen;

    private static GameManager _instance;
    private List<int> _sabotages = new List<int>();            // List of IDs for all possible sabotages
    private List<string> _sandomEventIds = new List<string>(); // Corresponds to Event-specific Dialogue Ids
    private int _sabotageId;   // The ID of the day's major event/sabotage

    private const int _maxNumSabotages = 6; // UPDATE WHEN ADDING NEW SABOTAGES TO JSON FILES

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        _day = 0;   // TODO: Reset to 0 after testing.

        for (int i = 1; i <= _maxNumSabotages; i++) {
            _sabotages.Add(i);
        }

        AdvanceDay();
    }

    public void AdvanceDay() {
        _day++;

        Sabotage.SabotageActive = true;

        if (_sabotages.Count > 1) {
            _sabotageId = _sabotages[Random.Range(0, _sabotages.Count)];

		} else if (_sabotages.Count == 1) {
            _sabotageId = _sabotages[0];

        } else {
            _screen.gameObject.SetActive(true);
            // SabotageId = 7;
            return;
        }

        _tabletMgr.Refresh();
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Alert);
        _sabotages.Remove(SabotageId);

        foreach (NPC npc in _npcList) { 
            npc.HasBeenSpokenTo = false;
        }
    }

    public void ClearSabotage() {
        Sabotage.SabotageActive = false;
    }

    public int Day {
        get { return _day; }
    }

    public int SabotageId {
        get { return _sabotageId; }
    }
}
