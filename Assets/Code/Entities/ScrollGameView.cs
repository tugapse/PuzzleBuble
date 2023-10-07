using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolllGameView : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    Vector3 startPosition;
    private void Awake()
    {
        levelManager.onLevelChanged += this.OnLevelChanged;
        levelManager.OnScrollView += this.OnScrollView;
        this.startPosition = this.transform.position;
    }

    private void OnScrollView(Vector2 scroll)
    {
        this.transform.position = (Vector2)this.startPosition + scroll;
    }

    private void Start()
    {
        this.transform.position = this.startPosition;
    }

    private void OnLevelChanged(Level level)
    {
        this.transform.position = startPosition;
    }
}
