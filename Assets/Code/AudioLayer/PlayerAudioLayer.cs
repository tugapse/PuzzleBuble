using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAudioLayer : MonoBehaviour
{
    public PlayerData playerData;
    public GridData gridData;

    [Header("Audio Sources")]
    public AudioSource shootAudio;
    public AudioSource explodeBuble;

    // Start is called before the first frame update
    void Start()
    {
        playerData.OnShoot += this.OnShoot;
        gridData.OnBallExplode += this.onBallExplode;
    }

    private void onBallExplode(Collider2D[] arg0)
    {
        if (this.explodeBuble != null) this.explodeBuble.Play();
    }

    private void OnShoot(Vector3 arg0)
    {
        if (this.shootAudio != null) this.shootAudio.Play();

    }



    // Update is called once per frame
    void Update()
    {

    }
}
