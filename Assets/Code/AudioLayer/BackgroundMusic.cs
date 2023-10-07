using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] LevelManager levelManager;

    [SerializeField] AudioSettings audioSettings;


    [SerializeField] float normalPitch = 1f;
    [SerializeField] float warningPitch = 1.4f;

    void Awake()
    {
        this.levelManager.OnNormalState += this.OnNormalState;
        this.levelManager.OnWarningState += this.OnWarningState;
        this.levelManager.OnGameOver += this.onGameOver;
    }

    void Start()
    {
        this.audioSource.volume = audioSettings.BGMVolume;
    }

    private void onGameOver()
    {
        this.audioSource.Stop();
    }

    private void OnWarningState()
    {
        this.audioSource.pitch = this.warningPitch;
    }

    private void OnNormalState()
    {
        this.audioSource.pitch = this.normalPitch;

    }


}
