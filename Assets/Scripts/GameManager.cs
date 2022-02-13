using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private int _day;
    public static string[] _randomEventIds;    // Corresponds to Event-specific Dialogue Ids
    public static int SabotageId;

    private static GameManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
