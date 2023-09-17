
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public GameGrid gamegrid;
    public Transform arrow;
    public Transform mira;
    float currentRotation = 0;

    public BallSpawner spawnner;
    public bool canShoot { get; set; } = false;
    public PlayerManager playerManager;
    public LevelManager levelManager;



    void Start()
    {
    }

    void Update()
    {
        if (!levelManager.GameRunning) return;
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
                playerManager.Shoot(transform.up);
            }
        }
    }

    void CheckHandleRotation()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        this.currentRotation += rotation * this.playerManager.RotationSpeed * Time.deltaTime;
        this.currentRotation = this.clampRotation(this.currentRotation);
        this.transform.rotation = Quaternion.Euler(0, 0, -this.currentRotation);
        if (rotation != 0) playerManager.Turn(-this.currentRotation);
    }
    float clampRotation(float rotaion)
    {
        return Mathf.Max(-this.playerManager.maxRotationAngle, Mathf.Min(this.playerManager.maxRotationAngle, rotaion));

    }
}
