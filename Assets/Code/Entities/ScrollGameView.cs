using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolllGameView : MonoBehaviour
{
    [SerializeField] float Speed = 0f;
    [SerializeField] LevelManager levelManager;
    Vector3 startPosition;
    private void Awake()
    {
        levelManager.onLevelChanged += this.OnLevelChanged;
        this.startPosition = this.transform.position;
    }

    private void OnLevelChanged(Level level)
    {
        this.transform.position = startPosition;
        this.Speed = level.levelSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (levelManager?.LevelRunning == false) return;
        this.transform.position = transform.position + Vector3.down * (Time.deltaTime * Speed);

    }
}
