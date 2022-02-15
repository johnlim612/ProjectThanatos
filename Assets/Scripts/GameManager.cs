using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    public static List<int> Sabotages = new List<int>();            // List of IDs for all possible sabotages
    public static List<string> RandomEventIds = new List<string>(); // Corresponds to Event-specific Dialogue Ids
    public static int? SabotageId;   // The ID of the day's major event/sabotage

    private const int _maxNumSabotages = 6; // UPDATE WHEN ADDING NEW SABOTAGES TO JSON FILES

    [SerializeField] private static int _day;

    private static GameManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _day = 0;

        for (int i = 1; i <= _maxNumSabotages; i++) {
            Sabotages.Add(i);
        }

        AdvanceDay();
    }

    public static void AdvanceDay() {
        _day += 1;
        // at the end of each day, you need to remove the OLD ID from the Sabotages list
        // then generate new ID:
        SabotageId = Sabotages[Random.Range(1, Sabotages.Count - 1)];
        
        Sabotage.SabotageActive = true;
    }

    public static void ClearSabotage() {
        Sabotage.SabotageActive = false;
    }
}
