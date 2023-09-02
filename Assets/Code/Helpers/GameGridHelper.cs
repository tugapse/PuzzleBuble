using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGridHelper
{
    static public Vector3 getGridPosition(Vector3 worldPosition, float offset = 0.5f)
    {
        float x = worldPosition.x;

        if (x == -1)
        {
            return new Vector3(-1 + offset, (int)worldPosition.y - offset, (int)worldPosition.z);
        }
        else if (x < 0) x--;

        return new Vector3((int)x + offset, (int)worldPosition.y - offset, (int)worldPosition.z);
    }

}
