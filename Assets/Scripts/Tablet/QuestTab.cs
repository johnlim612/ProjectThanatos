public class QuestTab : TabletManager {
    public void OpenTab() {
        int index = 1;
        string log = "";

        foreach (string str in _questLog) {
            log += $"{index++}: {str} ";
        }
        _screenText.text = log;
    }
}
