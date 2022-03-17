using UnityEngine;

public class Sabotage : MonoBehaviour {
    [SerializeField] private GameObject _thermometer;
    [SerializeField] private GameObject _electrical;
    [SerializeField] private GameObject _oxygen;

    public static bool SabotageActive = false;
}
