using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextEnd1 : MonoBehaviour
{
    public Text UIText;

    public void clearTxt() {
        UIText.text = " ";
    }

    public void changeTextFailed() {
        UIText.text = "They failed me.";
    }

    public void changeTextAnger() {
        UIText.text = "How dare they lie to me...";
    }

    public void quit() {
        Application.Quit();
    }
}
