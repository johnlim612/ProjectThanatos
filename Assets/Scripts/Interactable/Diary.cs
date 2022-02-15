public class Diary : InteractableObject {
    private string _diaryUnusable = "You need to repair the problem on the spaceship before you can use the diary";
    private string _diaryUsed = "You wrote on the diary of today's events";
    public string DiaryUsed {
        get { return _diaryUsed; }
	}

    public override void InteractObject() {
        if (GameManager.SabotageId != null) {
            print(_diaryUnusable);
        } else {
            OpenDiary();
        }
    }

    private void OpenDiary() {
        //FindObjectOfType<UI.UIDialogueManager>().StartDiaryDialogue(this);
        EndDay();
    }

    private void EndDay() {
        GameManager.AdvanceDay();
    }
}
