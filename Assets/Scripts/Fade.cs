using UnityEngine;

public class Fade : MonoBehaviour {
    [SerializeField] private GameObject _fade;
    private Animator _fadeController;

    private void Awake() {
        _fadeController = _fade.GetComponent<Animator>();
    }

    public void FadeOut() {
        _fadeController.SetBool("FadeOut", true);
    }

    public void FadeIn() {
        _fadeController.SetBool("FadeOut", false);
    }

    public void FadeActive(bool status) {
        _fade.SetActive(status);
    }
}
