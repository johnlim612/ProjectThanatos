public class DiaryTab : TabletManager {
    public void OpenTab() {
        // TODO: check if player night is over before
        // displaying the lastest diary entry.
        _screenText.text = _storedDiaryEntry;
    }
}
