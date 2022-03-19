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

    void nextQue() {
        activateTriggerOnNPC();
        activateTriggerOnSabotage();
        activateTriggerOnNPC();
        activateTriggerOnNPC();
    }

    void QueMethods() {
        _questQue.Enqueue(() => activateTriggerOnNPC());
    }
}
