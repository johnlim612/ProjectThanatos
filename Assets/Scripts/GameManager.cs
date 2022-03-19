using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private List<NPC> _npcList = new List<NPC>();
    [SerializeField] private TabletManager _tabletMgr;
    [SerializeField] private static int _day;

    public GameObject screen;

    private static GameManager _instance;
    public static List<int> Sabotages = new List<int>();            // List of IDs for all possible sabotages
    public static List<string> RandomEventIds = new List<string>(); // Corresponds to Event-specific Dialogue Ids
    public static int SabotageId;   // The ID of the day's major event/sabotage

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
            Sabotages.Add(i);
        }

        AdvanceDay();
    }

    public void AdvanceDay() {
        _day++;

        Sabotage.SabotageActive = true;

        if (Sabotages.Count > 1) {
            SabotageId = Sabotages[Random.Range(0, Sabotages.Count)];

		} else if (Sabotages.Count == 1) {
            SabotageId = Sabotages[0];

        } else {
            screen.gameObject.SetActive(true);
            // SabotageId = 7;
            return;
        }

        _tabletMgr.Refresh();
        //FindObjectOfType<UI.UIDialogueManager>().StartAnnouncement();
        Sabotages.Remove(SabotageId);

        //foreach (NPC npc in _npcList) {
        //    npc.HasBeenSpokenTo = false;
        //}
    }

    public static void ClearSabotage() {
        Sabotage.SabotageActive = false;
    }
}
