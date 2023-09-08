
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public GameGrid gamegrid;
    public float rotationSpeed = 0.2f;
    public AnimationCurve speedCurve;
    public float maxRotation = 30f;
    public Transform arrow;
    public Transform mira;
    float currentRotation = 0;

    public BallSpawner spawnner;
    public bool canShoot { get; set; } = false;



    void Start()
    {
        this.spawnner.Spawn();
    }

    void Update()
    {
        if (!gamegrid.gameStarted) return;
        this.CheckHandleRotation();
        this.handleJump();
    }
    void handleJump()
    {
        if (Input.GetButtonDown("Jump") && canShoot)
        {
            if (this.canShoot)
            {
                canShoot = false;
                this.spawnner.Shoot(this.transform.up);
                this.spawnner.Spawn();
            }
        }
    }

    void CheckHandleRotation()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        this.currentRotation += rotation * this.rotationSpeed * Time.deltaTime;
        this.currentRotation = this.clampRotation(this.currentRotation);
        this.transform.rotation = Quaternion.Euler(0, 0, -this.currentRotation);
    }
    float clampRotation(float rotaion)
    {
        return Mathf.Max(-this.maxRotation, Mathf.Min(this.maxRotation, rotaion));

    }
}
