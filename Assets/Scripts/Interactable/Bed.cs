using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject {

    [SerializeField] private string[] _descriptions;
    [SerializeField] private string[] _requirements;
    [SerializeField] private TabletController _tabletController;
    private Queue<(string, string)> _descriptionQueue;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            EndDay();
        }
    }

    public override void InteractObject() {
        if (Sabotage.SabotageActive) { // move bool to GM?
            // if sabotage isn't active AND quest isn't active... same thing?
            AddToQueue(_requirements);
            //FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Diary, this);
            print(_requirements[0]);
        } else {
            //OpenDiary();
            EndDay();
        }
    }

    //private void OpenDiary() {
    //    AddToQueue(_descriptions);
    //    FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Diary, this);
    //    StartCoroutine(WaitForDiary());
    //}

    //private IEnumerator WaitForDiary() {
    //    while (UI.UIDialogueManager.IsInteracting) {
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(1f);
    //    EndDay();
    //}

    private void EndDay() {
        // Create transition/animation when ending the day.
        StartCoroutine(WaitForDiaryEntry());
        // GameManager."fade to black" animation at EOD?
        print("EOD! You sneep.");
    }

    private IEnumerator WaitForDiaryEntry() {
        yield return StartCoroutine(_tabletController.UpdateDiary());
        GameManager.Instance.EndDay();
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
