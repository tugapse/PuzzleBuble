using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Ball : MonoBehaviour
{


    public int color = -1;
    public CircleCollider2D trigger;
    public List<Ball> conections = new List<Ball>();
    public List<Ball> sameColorConections = new List<Ball>();
    public GridCell parentCell = null;

    public LevelManager levelManager;
    public Color ExplosionColor;



    void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Top")
        {
            if (!this.PrepareBigidBody() || this.levelManager.GameStarted == false) return;

            GridCell cell = this.Snap();
            if (cell == null) return;
            this.trigger.enabled = true;
            this.CheckIsTopRow(cell, other);
            this.levelManager.RemoveConnected(cell);

        }
    }
    private void CheckIsTopRow(GridCell cell, Collision2D other)
    {
        if (levelManager.GameStarted)
        {
            cell.isTopRow = other.gameObject.tag == "Top";
        }
    }
    private bool PrepareBigidBody()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if (rb.bodyType == RigidbodyType2D.Kinematic) return false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Top")
        {
            var ball = other.gameObject.GetComponent<Ball>();
            if (ball == null) return;
            if (!this.conections.Contains(ball))
            {
                this.conections.Add(ball);
                if (ball.color == this.color) sameColorConections.Add(ball);
            }

        }
    }

    public GridCell Snap()
    {
        GridCell cell = levelManager.CurrentGrid.GetGridPosition(this.transform.position);
        if (cell != null)
        {
            cell.ball = this;
            this.parentCell = cell;
            this.transform.position = cell.gridPosition;
            this.gameObject.transform.parent = levelManager.CurrentGrid.transform;
        }
        return cell;
    }
    public void markToExplode()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        sp.color = new Vector4(sp.color.r, sp.color.g, sp.color.b, 0.5f);
    }


    private void OnDrawGizmosSelected()
    {
        // if (drawcSameColorConnections)
        foreach (var ball in this.sameColorConections)
        {
            if (ball == null)
            {
                continue;
            }
            UnityEngine.Gizmos.color = Color.black;
            UnityEngine.Gizmos.DrawLine(ball.transform.position, this.transform.position);
        }
        // if (drawConnections)
        foreach (var ball in this.conections)
        {
            if (ball == null)
            {
                continue;
            }
            UnityEngine.Gizmos.color = Color.black;
            UnityEngine.Gizmos.DrawLine(ball.transform.position, this.transform.position);
        }
    }


}
