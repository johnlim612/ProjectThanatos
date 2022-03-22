using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1 : Quest {

    /// <summary>
    /// EXAMPLE QUEST
    /// </summary>
    protected override void QueTriggers() {
        // Add diary start
        QuestQue.Enqueue(() => TriggerSabotage());
        QuestQue.Enqueue(() => TriggerMonologue(true));
        QuestQue.Enqueue(() => TriggerNPC(RachelLumina));
    }



    //QuestQue.Enqueue(() => TriggerNPC(JohnnyWalker));

}
