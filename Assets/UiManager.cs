using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public int playerPoints = 0;
    public TextMeshProUGUI text;
    public LevelManager levelManager;
    public PlayerManager playerManager;

    private void Start()
    {
        this.playerManager.OnScorePointsChanged += this.OnScorePointsChanged;
        this.playerPoints = 0;
    }

    private void OnScorePointsChanged(int points)
    {
        this.playerPoints += points;
        this.text.text = playerPoints.ToString();
    }

}
