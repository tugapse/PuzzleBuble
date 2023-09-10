
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public GameGrid gamegrid;
    public Transform arrow;
    public Transform mira;
    float currentRotation = 0;

    public BallSpawner spawnner;
    public bool canShoot { get; set; } = false;
    public PlayerData playerData;



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
                playerData.Shoot(transform.up);
            }
        }
    }

    void CheckHandleRotation()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        this.currentRotation += rotation * this.playerData.RotationSpeed * Time.deltaTime;
        this.currentRotation = this.clampRotation(this.currentRotation);
        this.transform.rotation = Quaternion.Euler(0, 0, -this.currentRotation);
        if (rotation != 0) playerData.Turn(-this.currentRotation);
    }
    float clampRotation(float rotaion)
    {
        return Mathf.Max(-this.playerData.maxRotationAngle, Mathf.Min(this.playerData.maxRotationAngle, rotaion));

    }
}
