using UnityEngine;

public class EndScene : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (GameManager.Instance.Day == Constants.BodyFoundDay+1) {
            GameManager.Instance.AdvanceDay();
        }
    }
}
