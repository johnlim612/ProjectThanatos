using UnityEngine;

/// <summary>
/// Attach script to the Calendar prefab.
/// </summary>
public class CalendarManager : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshPro _calendarText;
    private int _daysLeft; // Total # Days in-game (+1 for intro cutscene)

    private void Start() {
        _daysLeft = 8; // UPDATE THIS IF TOTAL # DAYS IN-GAME CHANGES
        _calendarText.text = _daysLeft.ToString();
    }

    public void SetNumDaysLeft() {
        _daysLeft--;
        _calendarText.text = _daysLeft.ToString();
    }
}
