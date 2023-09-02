using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GridCell
{
    public bool isEmpty { get { return ball != null; } }
    public bool isTopRow { get; set; }
    public Vector3 gridPosition;
    public Ball ball;

    public bool isRange(Vector3 worldPosition, float threashHold = 0.5f)
    {
        float diff = MathF.Abs(Vector2.Distance(worldPosition, this.gridPosition));
        return diff <= threashHold;
    }

    public void Clear()
    {
        GameObject.Destroy(ball.gameObject);
        this.ball = null;
    }
}
