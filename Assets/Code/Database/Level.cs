using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Level")]
public class Level : ScriptableObject
{
    public int StartLevelDelay;
    public GameObject[] availableBalls;

}
