using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    
    public Quest[] Quests;
    public static QuestManager Instance { get {return _instance; } }
    private static QuestManager _instance;

	private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
			_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        Quests = new Quest[] {
            new QuestDay1(),
            new QuestDay2(),
            new QuestDay3()
        };
    }

    public void TriggerNext() {
        Action action = Quests[GameManager.Instance.Day].QuestQue.Dequeue();
        action();
	}
}
