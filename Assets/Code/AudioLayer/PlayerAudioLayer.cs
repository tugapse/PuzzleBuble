using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class PlayerAudioLayer : MonoBehaviour
{

    public PlayerManager playerData;
    public LevelManager levelManager;
    public float minTurnAngleToPlaySound = 2;

    [Header("Audio Sources")]
    public AudioSource shootAudio;
    public AudioSource turn;
    public AudioSource explodeBuble;

    // Start is called before the first frame update
    void Start()
    {
        playerData.OnShoot += this.OnShoot;
        playerData.OnTurn += this.OnTurn;
        levelManager.OnBallExplode += this.onBallExplode;
    }

    private float lastTurnedAngle = 0;
    private void OnTurn(float angle)
    {
        float diff = Mathf.Abs(lastTurnedAngle - angle);
        if (diff >= this.minTurnAngleToPlaySound && this.turn != null)
        {
            this.lastTurnedAngle = angle;
            this.turn.Play();
        }
    }

    private void onBallExplode(GridCell[] cells)
    {
        if (this.explodeBuble != null) this.explodeBuble.Play();
    }

    private void OnShoot(Vector3 arg0)
    {
        if (this.shootAudio != null) this.shootAudio.Play();

    }

}
