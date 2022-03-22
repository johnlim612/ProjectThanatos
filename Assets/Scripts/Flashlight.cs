public class Flashlight : Item {
    public override void InteractObject() {
        base.InteractObject();
        Obtained = true;
        print("Flashlight obtained: " + Obtained);
    }
}
