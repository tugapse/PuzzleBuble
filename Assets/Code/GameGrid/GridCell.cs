using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class GridCell
{
    public bool isEmpty { get { return ball != null; } }
    public bool isTopRow { get; set; }
    public Vector3 gridPosition;
    public Ball ball;
    public List<GridCell> connectedCells
    {
        get
        {
            List<GridCell> result = new List<GridCell>();
            if (this.ball == null) return result;
            foreach (Ball ball in this.ball.conections)
            {
                var cell = GameGrid.current.GetGridPosition(ball.transform.position);
                result.Add(cell);
            }
            return result;
        }
    }

    public bool ToCheck { get; set; }

    public bool isRange(Vector3 worldPosition, float threashHold = 0.5f)
    {
        float diff = MathF.Abs(Vector2.Distance(worldPosition, this.gridPosition));
        return diff <= threashHold;
    }

    public void Clear()
    {
        this.ClearConnections();
        GameObject.Destroy(ball.gameObject);
        this.ball = null;
    }

    private void ClearConnections()
    {
        foreach (var cell in this.connectedCells)
        {
            cell.RemoveConnection(this);
        }
    }

    private void RemoveConnection(GridCell gridCell)
    {
        Ball b = gridCell.ball;
        if (this.ball && b) this.ball.conections.Remove(b);
    }

    public void Debug()
    {
        if (this.isTopRow)
        {
            var sp = this.ball.GetComponent<SpriteRenderer>();
            sp.color -= new Color(0, 0, 0, 0.5f);
        }
    }
}
