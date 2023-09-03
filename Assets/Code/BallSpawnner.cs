using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    public GameObject[] Balls;
    public float shootForce = 40f;
    public Transform nextBallTranform;
    GameObject currentBall;
    GameObject nextBall;
    public float maxCooldown = 120;
    public Color GizmoColor = Color.cyan;

    public void Spawn()
    {
        this.currentBall = this.nextBall;
        if (this.currentBall == null)
        {
            this.currentBall = this.Balls[Random.Range(0, Balls.Length)];
        }
        this.nextBall = this.Balls[Random.Range(0, Balls.Length)];
        this.SwapSprites();
    }

    private void SwapSprites()
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.color = this.currentBall.GetComponent<SpriteRenderer>().color;
        SpriteRenderer nextSpriteRenderer = this.nextBallTranform.GetComponent<SpriteRenderer>();
        nextSpriteRenderer.color = this.nextBall.GetComponent<SpriteRenderer>().color;
    }

    public bool Shoot(Vector3 direction)
    {
        if (!PlayerControler.current.canShoot) return false;
        var ball = Instantiate(this.currentBall, this.transform.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * this.shootForce, ForceMode2D.Impulse);
        return true;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = this.GizmoColor;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
        Gizmos.DrawWireSphere(this.nextBallTranform.position, 0.5f);
    }
    public GameObject InstanciateBall(Vector3 position)
    {
        return Instantiate(this.Balls[Random.Range(0, Balls.Length )], position, Quaternion.identity);
    }
}
