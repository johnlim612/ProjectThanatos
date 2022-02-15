using UnityEngine;

public class HallwayManager : MonoBehaviour {
    [SerializeField] private bool _enableHallwayTransition; 
    
    public bool EnableHallwayTransition {
        get { return _enableHallwayTransition; }
    }
}