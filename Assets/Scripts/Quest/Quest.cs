using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest : MonoBehaviour
{
    public NPC WonKi, JohnnyWalker, RachelLumina, YuriMiko;

    public Queue<Action> QuestQue { get; set; }
    public string CurrentQuest;

	// Start is called before the first frame update
	void Awake() {
        QuestQue = new Queue<Action>();
    }

	void Start() {
        QueTriggers();
	}

    protected virtual void QueTriggers() {
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    protected void TriggerNPC(NPC npc, Item ReqItem = null) {
        if (ReqItem != null && !ReqItem.Obtained) {
            return;
		}
        npc.ActiveQuest = true;
	}

    protected void TriggerSabotage() {
        Sabotage.SabotageActive = true;
    }


}
