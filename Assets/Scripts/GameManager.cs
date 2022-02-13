using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private static int _day { get; set; }
    [SerializeField] private int[] _ongoingEventIds;    // Corresponds to Event-specific Dialogue Ids

    private static GameManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void advanceDay() {
        _day += 1;
    }
}
