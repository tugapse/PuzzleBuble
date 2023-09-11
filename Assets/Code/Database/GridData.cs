using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Database/Grid")]
public class GridData : ScriptableObject
{
    private GameGrid currentGrid;

    public GameGrid CurrentGrid { get { if (currentGrid == null) currentGrid = GameObject.FindFirstObjectByType<GameGrid>(); return this.currentGrid; } }
    public UnityAction<Collider2D[]> OnBallCollision;
    public UnityAction<GridCell[]> OnBallExplode;
    public UnityAction<GridCell> OnRemoveConnected;
    public UnityAction OnWarningState;
    public UnityAction OnNormalState;

    public void BallCollision()
    {
        if (this.OnBallCollision != null) this.OnBallCollision(null);
    }


    public void BallExplode(List<GridCell> balls)
    {
        if (this.OnBallExplode != null) this.OnBallExplode(balls.ToArray());
    }

    public void RemoveConnected(GridCell cell)
    {
        if (this.OnRemoveConnected != null) this.OnRemoveConnected(cell);
    }

    public void WarningState()
    {
        if (this.OnWarningState != null) this.OnWarningState();

    }

    public void NormalState()
    {
        if (this.OnNormalState != null) this.OnNormalState();

    }
}
