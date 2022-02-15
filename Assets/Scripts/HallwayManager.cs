using UnityEngine;

public class HallwayManager : MonoBehaviour {
    [SerializeField] private bool _enableHallwayTransition;
    [SerializeField] private string _mapSceneName;
    
    public bool EnableHallwayTransition {
        get { return _enableHallwayTransition; }
    }

    public string MapSceneName {
        get { return _mapSceneName; }
    }
}
