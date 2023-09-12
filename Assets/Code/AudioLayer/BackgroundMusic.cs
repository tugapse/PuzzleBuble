using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public GridData gridData;


    public float normalPitch = 1f;
    public float warningPitch = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        this.gridData.OnNormalState += this.OnNormalState;
        this.gridData.OnWarningState += this.OnWarningState;
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
