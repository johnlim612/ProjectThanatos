using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest : MonoBehaviour
{
    protected Queue<Action> _questQue;
    public string CurrentQuest;

    


	// Start is called before the first frame update
	void Awake() {
        _questQue = new Queue<Action>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void TriggerNPC(NPC npc, Item ReqItem = null) {
        if (ReqItem != null && !ReqItem.Obtained) {
            return;
		}

        npc.ActiveQuest = false;
	}



}
