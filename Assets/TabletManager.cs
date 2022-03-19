using TMPro;
using UI;
using UnityEngine;

public abstract class TabletManager : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI _screenText;

    private string[] _data;
    private int _sabotageID;

    public void Refresh() {
        _sabotageID = GameManager.SabotageId;
        DialogueDataManager.Instance.Initialize(UI.EntityType.Diary, "Tablet", _sabotageID);

        //string str = DialogueDataManager.Instance.GetDiaryEntry();
        //string[] questLog = DialogueDataManager.Instance.GetQuestLog();
    }

    public abstract void OpenTab();

    public string[] Data { get; }
}