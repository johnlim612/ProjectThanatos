using UnityEngine;
using UnityEngine.UI;

public class TabletButton : MonoBehaviour {
    [SerializeField] private ButtonType _type;
    [SerializeField] private Button _button;

    private static ColorBlock _selectedColorBlock;
    private static ColorBlock _unselectedColorBlock;
    private static TabletButton _selectedButton;

    public void Select() {
        if (_selectedButton != null) {
             _selectedButton.Button.colors = _unselectedColorBlock;
        }

        _button.colors = _selectedColorBlock;
        _selectedButton = this;
    }

    public bool IsSelected() {
        return _selectedButton == this;
    }

    public static void SetSelectedColorBlock(ColorBlock color) {
        _selectedColorBlock = color;
    }

    public static void SetUnselectedColorBlock(ColorBlock color) {
        _unselectedColorBlock = color;
    }

    public ButtonType Type {
        get { return _type; }
    }

    public Button Button {
        get { return _button; }
    }
}

public enum ButtonType {
    Quest,
    Diary,
    Map,
    Won,
    Rachel,
    Yuri,
    Johnny
}