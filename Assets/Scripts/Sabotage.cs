using UnityEngine;

public class Sabotage : MonoBehaviour {
    [SerializeField] private GameObject _thermometer;
    [SerializeField] private GameObject _electrical;
    [SerializeField] private GameObject _oxygen;

    private void Update() {
        switch (GameManager.SabotageId) {
            case 1:
                _thermometer.SetActive(true);
                _electrical.SetActive(false);
                _oxygen.SetActive(false);
                break;
            case 2:
                _thermometer.SetActive(false);
                _electrical.SetActive(true);
                _oxygen.SetActive(false);
                break;
            case 3:
                _thermometer.SetActive(false);
                _electrical.SetActive(false);
                _oxygen.SetActive(true);
                break;
            default:
                _thermometer.SetActive(false);
                _electrical.SetActive(false);
                _oxygen.SetActive(false);
                break;
        }
    }
}
