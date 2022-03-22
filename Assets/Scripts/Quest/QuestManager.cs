using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    
    public Quest[] Quests;
    public static QuestManager Instance { get {return _instance; } }
    private static QuestManager _instance;
    public int currentQuestNum = 0;

	private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
			_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void TriggerNext() {
        if (Quests[GameManager.Instance.Day - 1].QuestQue.Count == 0) {
            GameManager.Instance.CurrentSabotage.ToggleActiveState();
            currentQuestNum = 0;
            return;
		} 

        currentQuestNum += 1;
        Action action = Quests[GameManager.Instance.Day].QuestQue.Dequeue();
        action();
	}

    public string getCurrentQuest () {
        return Quests[GameManager.Instance.Day].QuestLog[currentQuestNum];
    }
}
