using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1 : Quest {

	void Start() {
        QuestLog = new string[] {"", "", "Rachel", "Yuri", "Johnny", "Wonki", "Wonki" , "Wonki", "Wonki" };
    }

    protected override void QueTriggers() {
        // Add diary start
        QuestQue.Enqueue(() => TriggerSabotage());
        QuestQue.Enqueue(() => TriggerMonologue());
        QuestQue.Enqueue(() => TriggerNPC(RachelLumina));
        QuestQue.Enqueue(() => TriggerNPC(YuriMiko));
        QuestQue.Enqueue(() => TriggerNPC(JohnnyWalker));
        QuestQue.Enqueue(() => TriggerNPC(WonKi));
        QuestQue.Enqueue(() => TriggerNPC(WonKi));
        QuestQue.Enqueue(() => TriggerNPC(WonKi, GameObject.Find("FlashLight").GetComponent<Item>()));
        // Diary End
        // Diary Write

    }
}
