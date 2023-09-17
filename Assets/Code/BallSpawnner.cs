using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    [SerializeField] Transform nextBallTranform;
    [SerializeField] BallSpawnerAnimations animations;
    private GameObject currentBall;
    private GameObject nextBall;
    [SerializeField] PlayerControler playerControler;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] LevelManager levelManager;
    private Level level;

    Color GizmoColor = Color.cyan;

    void Start()
    {
        this.playerManager.OnShoot += this.OnPlayerShoot;
        this.levelManager.onLevelChanged += this.OnLevelChanged;

    }

    private void OnLevelChanged(Level newLevel)
    {
        this.level = newLevel;
        this.Spawn();
    }

    private void OnPlayerShoot(Vector3 direction)
    {
        var ballObject = Instantiate(this.currentBall, this.transform.position, Quaternion.identity);
        ballObject.GetComponent<Ball>().levelManager = this.levelManager;
        Rigidbody2D rb = ballObject.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * this.playerManager.shootForce, ForceMode2D.Impulse);
        this.Spawn();

    }

    public void Spawn()
    {
        this.currentBall = this.nextBall;

        if (this.currentBall == null)
        {
            this.currentBall = this.level.availableBalls[Random.Range(0, this.level.availableBalls.Length)];
        }
        this.nextBall = this.level.availableBalls[Random.Range(0, this.level.availableBalls.Length)];
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
        ball.GetComponent<Ball>().levelManager = this.levelManager;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * this.playerManager.shootForce, ForceMode2D.Impulse);

    }

    void OnDrawGizmosSelected()
    {
        UnityEngine.Gizmos.color = this.GizmoColor;
        UnityEngine.Gizmos.DrawWireSphere(this.transform.position, 0.5f);
        UnityEngine.Gizmos.DrawWireSphere(this.nextBallTranform.position, 0.5f);
    }
    public GameObject InstanciateBall(Vector3 position)
    {
        return Instantiate(this.level.availableBalls[Random.Range(0, this.level.availableBalls.Length)], position, Quaternion.identity);
    }
}
