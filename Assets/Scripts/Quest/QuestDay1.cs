using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1 : Quest {

    /// <summary>
    /// EXAMPLE QUEST
    /// </summary>
    protected override void QueTriggers() {
        QuestQue.Enqueue(() => TriggerNPC(JohnnyWalker));
        QuestQue.Enqueue(() => TriggerNPC(WonKi));
        QuestQue.Enqueue(() => TriggerSabotage());
        QuestQue.Enqueue(() => TriggerNPC(YuriMiko));
        QuestQue.Enqueue(() => TriggerNPC(WonKi));
        QuestQue.Enqueue(() => TriggerNPC(JohnnyWalker));
    }
}
