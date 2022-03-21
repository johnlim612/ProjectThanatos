using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1 : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    /// <summary>
    /// EXAMPLE
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
