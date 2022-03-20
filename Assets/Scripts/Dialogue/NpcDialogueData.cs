using System.Collections.Generic;

namespace EntityData {
    struct DialogueReference {
        public string EventType;    // E.g. "sabotage", "random", etc.
        public string EventKey;     // E.g. if random event = "coffee", else an int "1"

        public DialogueReference(string eventType, string eventKey) {
            EventType = eventType;
            EventKey = eventKey;
        }
    }

    public class NpcDialogueData : EntityData {
        private string _greeting;    // First thing NPC displays when interacted with.
        private List<string> _prompts;   // Players' prompts to start different convos w/ NPCs
        private Queue<(string, string)> _backstoryDialogue;
        private Queue<(string, string)> _randEventDialogue;
        private Queue<(string, string)> _sabotageDialogue;

        public NpcDialogueData(string name, UI.EntityType dataType, string greeting,
                               List<string> prompts, Queue<(string, string)> backstory, 
                               Queue<(string, string)> rdmEvent, Queue<(string, string)> sabotage) {
            Name = name;
            DataType = dataType;
            _greeting = greeting;
            _prompts = prompts;
            _backstoryDialogue = backstory;
            _randEventDialogue = rdmEvent;
            _sabotageDialogue = sabotage;
        }

        public string GetGreeting() {
            return _greeting;
        }

        public List<string> GetPrompts() {
            return _prompts;
        }

        public Queue<(string, string)> GetDialogue(int selectedDialogue) {
            Queue<(string, string)> dialogue = new Queue<(string, string)>();
            return dialogue;
        }
    }
}
