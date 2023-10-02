using UnityEngine;

[CreateAssetMenu(menuName = "Database/Level")]
public class Level : ScriptableObject
{
    public int StartLevelDelay;
    public int levelSpeed;
    public int noiseOffsetX = 0;
    public int noiseOffsetY = 0;
    public float noiseScale = 0.1f;
    public int noiseRepetition = 1;
    public GameObject[] availableBalls;

}
