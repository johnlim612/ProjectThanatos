public class InteractableSabotage : InteractableObject {
    public override void InteractObject() {
        print("Sabotage Repaired");
        GameManager.ClearSabotage();
    }
}
