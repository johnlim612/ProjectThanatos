using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    
    public Quest[] Quests;

	void Awake() {
        Quests = new Quest[] {
            new QuestDay1(),
            new QuestDay2(),
            new QuestDay3()
        };
    }
	// Start is called before the first frame update
	void Start()
    {
        
    }
}
