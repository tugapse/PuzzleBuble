using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    [SerializeField] Transform nextBallTranform;
    [SerializeField] BallSpawnerAnimations animations;
    [SerializeField] PlayerControler playerControler;

    [Header("Managers")]
    [SerializeField] PlayerManager playerManager;
    [SerializeField] LevelManager levelManager;


    private GameObject currentBall;
    private GameObject nextBall;
    private Level currentLevel;

    Color GizmoColor = Color.cyan;

    void Awake()
    {
        this.playerManager.OnShoot += this.OnPlayerShoot;
        this.levelManager.onLevelChanged += this.OnLevelChanged;
    }


    private void OnLevelChanged(Level newLevel)
    {
        this.currentLevel = newLevel;
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
            this.currentBall = this.currentLevel.availableBalls[Random.Range(0, this.currentLevel.availableBalls.Length)];
        }
        this.nextBall = this.currentLevel.availableBalls[Random.Range(0, this.currentLevel.availableBalls.Length)];
        this.animations.StartAnimation();
    }

    // called by Ball Spanwer animations script
    private void SwapSprites()
    {
        SpriteRenderer currentSpriteRenderer = this.GetComponent<SpriteRenderer>();
        currentSpriteRenderer.sprite = this.currentBall.GetComponent<SpriteRenderer>().sprite;
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
        int ballsLength = this.currentLevel.availableBalls.Length;
        var noisepos = (position + currentLevel.noiseOffset) * currentLevel.noiseScale;
        float noiseValue = Mathf.PerlinNoise(noisepos.x, noisepos.y) * ballsLength * currentLevel.noiseRepetition;
        int index = Mathf.FloorToInt(noiseValue) % currentLevel.availableBalls.Length;
        return Instantiate(this.currentLevel.availableBalls[index], position, Quaternion.identity);
    }
}
