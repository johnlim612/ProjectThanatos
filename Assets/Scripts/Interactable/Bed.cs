using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject {

    [SerializeField] private string[] _descriptions;
    [SerializeField] private string[] _requirements;
    private Queue<(string, string)> _descriptionQueue;

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
        GameManager.Instance.AdvanceDay();
    }

    private IEnumerator WaitForDiaryEntry() {
        while (false) { // <--- CHANGE to "while !done animation"
            // call TabletManager's "end of day" method to trigger "update diary" animation
            yield return null;
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
