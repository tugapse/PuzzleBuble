using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    public GameGrid gameGrid;
    public GameObject[] Balls;
    public Transform nextBallTranform;
    public BallSpawnerAnimations animations;
    private GameObject currentBall;
    private GameObject nextBall;
    public PlayerControler playerControler;

    public PlayerData playerData;

    Color GizmoColor = Color.cyan;

    void Start()
    {
        this.playerData.OnShoot += this.OnPlayerShoot;
    }

    private void OnPlayerShoot(Vector3 direction)
    {
        var ball = Instantiate(this.currentBall, this.transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().gameGrid = this.gameGrid;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * this.playerData.shootForce, ForceMode2D.Impulse);
        this.Spawn();

    }

    public void Spawn()
    {
        this.currentBall = this.nextBall;

        if (this.currentBall == null)
        {
            this.currentBall = this.Balls[Random.Range(0, Balls.Length)];
        }
        this.nextBall = this.Balls[Random.Range(0, Balls.Length)];
        this.animations.StartAnimation();
    }

    private void SwapSprites()
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.currentBall.GetComponent<SpriteRenderer>().sprite;
        SpriteRenderer nextSpriteRenderer = this.nextBallTranform.GetComponent<SpriteRenderer>();
        nextSpriteRenderer.sprite = this.nextBall.GetComponent<SpriteRenderer>().sprite;
    }

    public void Shoot(Vector3 direction)
    {
        var ball = Instantiate(this.currentBall, this.transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().gameGrid = this.gameGrid;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * this.playerData.shootForce, ForceMode2D.Impulse);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = this.GizmoColor;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
        Gizmos.DrawWireSphere(this.nextBallTranform.position, 0.5f);
    }
    public GameObject InstanciateBall(Vector3 position)
    {
        return Instantiate(this.Balls[Random.Range(0, 4)], position, Quaternion.identity);
    }
}
