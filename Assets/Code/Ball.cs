using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool active = true;
    public bool toDestroy = false;
    public int color = -1;
    public CircleCollider2D trigger;
    public List<Ball> conections = new List<Ball>();
    public List<Ball> sameColorConections = new List<Ball>();
    public bool snapped = false;


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Top")
        {
            if (!this.PrepareBigidBody()) return;
            GridCell cell = this.Snap();
            if (cell == null) return;
            cell.isTopRow = other.gameObject.tag == "Top";
            GameGrid.current.RemoveConnected(cell);

        }
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


        GridCell cell = GameGrid.current.GetGridPosition(this.transform.position);
        if (cell != null)
        {
            cell.ball = this;
            this.transform.position = cell.gridPosition;
            PlayerControler.current.canShoot = true;
        }
        this.snapped = true;
        return cell;
    }
    private bool PrepareBigidBody()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        if (rb.bodyType == RigidbodyType2D.Static) return false;
        rb.bodyType = RigidbodyType2D.Static;
        this.trigger.enabled = true;
        return true;
    }

    private void OnDrawGizmos()
    {
        foreach (var ball in this.conections)
        {
            if (ball == null)
            {
                continue;
            }
            Gizmos.color = Color.black;
            Gizmos.DrawLine(ball.transform.position, this.transform.position);
        }
    }


}
