using UnityEngine;
/// <summary>
/// Saves the current and next position for the player when going through a hallway
/// </summary>
public static class HallwaySaveData {
    public static Vector3 CurrentPosition;
    public static Vector3 NewPosition;
    public static bool IsInitialized = false;
}
