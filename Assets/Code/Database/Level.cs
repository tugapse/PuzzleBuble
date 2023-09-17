using UnityEngine;

[CreateAssetMenu(menuName = "Database/Level")]
public class Level : ScriptableObject
{
    public int StartLevelDelay;
    public Vector3Int noiseOffset = Vector3Int.zero;
    public float noiseScale = 0.1f;
    public int noiseRepetition = 1;
    public GameObject[] availableBalls;

}
