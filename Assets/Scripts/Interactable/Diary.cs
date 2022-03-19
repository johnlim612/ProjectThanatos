using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : InteractableObject {

    [SerializeField] private string[] _descriptions;
    [SerializeField] private string[] _requirements;
    private Queue<(string, string)> _descriptionQueue;

    public override void InteractObject() {
        if (Sabotage.SabotageActive) {
            AddToQueue(_requirements);
            FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Diary, this);
        } else {
            OpenDiary();
        }
    }

    private void OpenDiary() {
        AddToQueue(_descriptions);
        FindObjectOfType<UI.UIDialogueManager>().InitializeDialogue(UI.EntityType.Diary, this);
        StartCoroutine(WaitForDiary());
    }

    private IEnumerator WaitForDiary() {
        while (UI.UIDialogueManager.IsInteracting) {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        EndDay();
    }

    private void EndDay() {
        // Create transition/animation when ending the day.
        GameManager.Instance.AdvanceDay();
    }

    public Queue<(string, string)> DescriptionQueue {
        get { return _descriptionQueue; }
    }

    public void AddToQueue(string[] sentences) {
        _descriptionQueue = new Queue<(string, string)>();
        foreach (string sentence in sentences) {
            _descriptionQueue.Enqueue(("Player", sentence));
        }
    }
}
