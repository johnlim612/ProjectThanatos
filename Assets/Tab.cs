using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour {
    [SerializeField] private TabType _tabType;

    private Button _button;
    private void Start() {
        _button = transform.parent.GetComponent<Button>();

        if (_button == null) {
            throw new NullReferenceException("Button component could not be found.");
        }
    }

    public TabType ButtonType { 
        get { return _tabType; }
    }

    public enum TabType {
        Quest,
        Diary,
        Map      
    }
}