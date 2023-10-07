
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    [SerializeField] PlayerManager playerManager;
    [SerializeField] LevelManager levelManager;
    PlayerInput playerInput;
    Vector2 rotationVector;
    bool fire;
    public bool canShoot { get; set; } = false;
    float currentRotation = 0;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value)
    {

        this.rotationVector = value.Get<Vector2>();
    }

    public void OnFire()
    {
        fire = true;
    }


    void Update()
    {
        if (!levelManager.LevelRunning) return;
        this.CheckHandleRotation();
        this.handleJump();
    }
    void handleJump()
    {
        if (fire && canShoot)
        {
            canShoot = false;
            fire = false;
            playerManager.Shoot(transform.up);

        }
    }

    void CheckHandleRotation()
    {
        float rotation = this.rotationVector.x;
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
