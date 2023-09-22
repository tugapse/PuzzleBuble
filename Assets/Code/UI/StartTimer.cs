using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class StartTimer : MonoBehaviour
{

    public TextMeshProUGUI text;

    [SerializeField] LevelManager levelManager;
    private void Start()
    {
        this.levelManager.OnLevelCount += this.onLevelCount;
        this.levelManager.onLevelStarted += this.onLevelStarted;
    }

    private void onLevelStarted(Level arg0)
    {
        text.enabled = false;
    }

    private void onLevelCount(float remainingTime)
    {
        int value = Mathf.FloorToInt(remainingTime);
        this.text.text = value == 0 ? "Joga" : value.ToString();
    }


}
