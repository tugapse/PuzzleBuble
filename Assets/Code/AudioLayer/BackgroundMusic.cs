using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public LevelManager levelManager;


    public float normalPitch = 1f;
    public float warningPitch = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        this.levelManager.OnNormalState += this.OnNormalState;
        this.levelManager.OnWarningState += this.OnWarningState;
    }

    private void OnWarningState()
    {
        Debug.Log("Warning State");
        this.audioSource.pitch = this.warningPitch;
    }

    private void OnNormalState()
    {
        this.audioSource.pitch = this.normalPitch;

    }


}
