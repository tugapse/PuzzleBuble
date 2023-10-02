
using System;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    [SerializeField] GameGrid gamegrid;
    [SerializeField] BallSpawner spawnner;
    [SerializeField] Transform pointerTransform;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] LevelManager levelManager;

    public bool canShoot { get; set; } = false;
    float currentRotation = 0;


    void Update()
    {
        if (!levelManager.LevelRunning) return;
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
