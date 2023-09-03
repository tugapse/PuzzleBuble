using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public static PlayerControler current { get; private set; }
    public float rotationSpeed = 0.2f;
    public float maxRotation = 30f;
    public Transform arrow;
    public Transform mira;
    float currentRotation = 0;

    public BallSpawner spawnner;
    public bool canShoot = true;


    private bool shoot = false;


    void Start()
    {
        PlayerControler.current = this;
        this.spawnner.Spawn();
    }

    void Update()
    {
        this.CheckHandleRotation();
        this.handleJump();
    }
    void FixedUpdate()
    {
    }

    void handleJump()
    {
        if (Input.GetButtonDown("Jump") && canShoot)
        {
            if (this.spawnner.Shoot(this.transform.up))
            {
                canShoot = false;
                this.spawnner.Spawn();
            }
        }
    }

    void CheckHandleRotation()
    {
        float rotation = Input.GetAxis("Horizontal");
        this.currentRotation += rotation * this.rotationSpeed;
        this.currentRotation = this.clampRotation(this.currentRotation);
        this.transform.rotation = Quaternion.Euler(0, 0, -this.currentRotation);
    }
    float clampRotation(float rotaion)
    {
        return Mathf.Max(-this.maxRotation, Mathf.Min(this.maxRotation, rotaion));

    }
}
