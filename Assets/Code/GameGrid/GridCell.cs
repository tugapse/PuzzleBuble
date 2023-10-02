
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridCell
{
    public bool isEmpty { get { return ball == null; } }
    public bool isFull { get { return ball != null; } }
    public bool isTopRow { get; set; }
    public Vector3 gridPosition;
    public Ball ball;
    public bool isDirty;
    public bool isFalling;
    public GameGrid gameGrid;
    public List<GridCell> connectedCells
    {
        get
        {
            List<GridCell> result = new List<GridCell>();
            if (this.isEmpty) return result;
            foreach (Ball ball in this.ball.conections)
            {
                var cell = gameGrid.GetGridPosition(ball.transform.position);
                result.Add(cell);
            }
            return result;
        }
    }


    public bool isRange(Vector3 worldPosition, float threashHold = 0.5f)
    {
        float diff = MathF.Abs(Vector2.Distance(worldPosition, this.gridPosition));
        return diff <= threashHold;
    }

    public void Clear(bool destroy = true)
    {
        this.ClearConnections();
        if (destroy && ball?.gameObject) ball.Explode();
        this.ball = null;
        this.isTopRow = false;
        this.isDirty = false;
    }

    private void ClearConnections()
    {
        foreach (var cell in this.connectedCells)
        {
            if (cell != null) cell.RemoveConnection(this);
        }
    }

    private void RemoveConnection(GridCell gridCell)
    {
        Ball b = gridCell.ball;
        if (this.ball && b) this.ball.conections.Remove(b);
    }

    public void Debug()
    {
        if (this.ball != null)
        {
            var sp = this.ball.GetComponent<SpriteRenderer>();
            sp.color -= new Color(0, 0, 0, 0.5f);
        }
    }

    public void Fall(float gravityMultiply = 1f)
    {
        this.isFalling = true;
        Rigidbody2D rb = this.ball.GetComponent<Rigidbody2D>();
        var colliders = this.ball.GetComponents<Collider2D>();
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = collider.isTrigger;
            }
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = Random.Range(5, 5 + gravityMultiply);
        this.SetBallOpacity();
        this.Clear(false);
    }
    public void Fall(Vector3 explosionPoint, float gravityMultiply = 1f)
    {
        var colliders = this.ball.GetComponents<Collider2D>();
        this.Explode(explosionPoint, gravityMultiply);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
        this.Clear(false);
    }

    private void Explode(Vector3 explosionPoint, float gravityMultiply = 1.9f, float force = 5f)
    {
        Rigidbody2D rb = this.ball.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;

        explosionPoint.Normalize();
        Vector3 random = Vector3.left * Random.Range(-5, +6);

        if (explosionPoint == gridPosition)
        {
            rb.AddForce(explosionPoint + Vector3.down * force + random.normalized, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(explosionPoint * force + random, ForceMode2D.Impulse);
        }
        rb.gravityScale = gravityMultiply;
        SetBallOpacity();
    }

    private void SetBallOpacity()
    {
        SpriteRenderer sp = ball.GetComponent<SpriteRenderer>();
        if (sp != null && sp.color.a == 1f)
        {
            sp.color = sp.color - new Color(0, 0, 0, 0.5f);
        }

    }
}
