using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    public static List<string> RandomEventIds = new List<string>(); // Corresponds to Event-specific Dialogue Ids
    public static int? SabotageId;   // The ID of the day's major event/sabotage

    [SerializeField] private static int _day;

    private static GameManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // REMOVE AFTER TESTING
        SabotageId = 1;
        RandomEventIds.Add("coffee");
        RandomEventIds.Add("laboratory");
    }

    public static void AdvanceDay() {
        _day += 1;
        SabotageId = Random.Range(1, 3);
    }

    public static void ClearSabotage() {
        SabotageId = null;
    }
}
