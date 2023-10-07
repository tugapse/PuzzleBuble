using UnityEngine;

[CreateAssetMenu(menuName = "Database/Level")]
public class Level : ScriptableObject
{
    public GameObject[] availableBalls;
    public int StartLevelDelay;
    public float levelSpeed;
    public int noiseOffsetX = 0;
    public int noiseOffsetY = 0;
    public float noiseScale = 0.1f;
    public int noiseRepetition = 1;
    public int minViewBallsBeforeScroll = 72;
}
