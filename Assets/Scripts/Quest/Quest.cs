using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest : MonoBehaviour {
    public NPC WonKi, JohnnyWalker, RachelLumina, YuriMiko;

    public Queue<Action> QuestQue { get; set; }
    public bool QuestComplete;


    // Start is called before the first frame update
    void Awake() {
        QuestQue = new Queue<Action>();
        QuestComplete = false;
    }

	void Start() {
        QueTriggers();
	}

    protected virtual void QueTriggers() {}

    protected void TriggerNPC(NPC npc, Item ReqItem = null) {
        if (ReqItem != null && !ReqItem.Obtained) {
            return;
		}
        npc.ActiveQuest = true;
	}

    protected void TriggerSabotage() {
        Sabotage.IsActive = true;
    }

    protected void TriggerDiary() {}

    protected void TriggerMonologue() {
        UI.UIDialogueManager.Instance.InitializeDialogue(UI.EntityType.Player);

	}

    protected void TriggerAlert() {}   

}
