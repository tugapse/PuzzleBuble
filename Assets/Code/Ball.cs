using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool active = true;
    public bool toDestroy = false;
    public int color = -1;


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Top")
        {
            GridCell cell = this.Snap();
            GameGrid.current.checkConnection(cell);

        }
    }

    public GridCell Snap()
    {

        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if (rb.bodyType == RigidbodyType2D.Static) return null;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector3.zero;
        GridCell cell = GameGrid.current.getGridPosition(this.transform.position);
        if (cell != null)
        {
            cell.ball = this;
            this.transform.position = cell.gridPosition;
            PlayerControler.current.canShoot = true;
        }
        return cell;
    }


}
