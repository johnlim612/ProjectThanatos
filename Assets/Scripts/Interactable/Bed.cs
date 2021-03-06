using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject {

    [SerializeField] private string[] _descriptions;
    [SerializeField] private string[] _requirements;
    [SerializeField] private TabletController _tabletController;
    private Queue<(string, string)> _descriptionQueue;

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.N)) {
        //    Sleep();
        //}
    }

    public override void InteractObject() {
        if (Sabotage.IsActive) { // move bool to GM?
            // if sabotage isn't active AND quest isn't active... same thing?
            AddToQueue(_requirements);
            //FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Diary, this);
            print(_requirements[0]);
        } else {
            //OpenDiary();
            Sleep();
        }
    }

    /// <summary>
    /// Trigger player's end-of-day diary entry, then end the day.
    /// </summary>
    private void Sleep() {
        StartCoroutine(WaitForDiaryEntry());
        print("EOD! You sneep.");
    }

    private IEnumerator WaitForDiaryEntry() {
        yield return StartCoroutine(_tabletController.UpdateDiaryCoroutine());
        if (GameManager.Instance.Day != Constants.BodyFoundDay) {
            GameManager.Instance.EndDay();
        }
    }

    public Queue<(string, string)> DescriptionQueue {
        get { return _descriptionQueue; }
    }

    public void AddToQueue(string[] sentences) {
        _descriptionQueue = new Queue<(string, string)>();
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue((Constants.PlayerKey, sentence));
        }
    }
}
