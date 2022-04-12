using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {
    public void ChangeScene(string name) {
        StopAllCoroutines();
        SceneManager.LoadScene(name);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
