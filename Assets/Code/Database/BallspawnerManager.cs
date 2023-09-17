using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class BallspawnerManager : ScriptableObject
{
    public UnityAction<Vector3, GameObject> onSpawnBall;

    public void SpawnBall(Vector3 worldPosition, GameObject objectToSpawn = null)
    {
        this.onSpawnBall?.Invoke(worldPosition, objectToSpawn);
    }



}
