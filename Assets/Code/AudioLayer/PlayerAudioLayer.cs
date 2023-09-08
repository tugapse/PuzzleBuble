using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAudioLayer : MonoBehaviour
{
    public PlayerData playerData;

    public AudioSource shootAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerData.OnShoot += this.OnShoot;
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
