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

    private void Start()
    {
        this.levelManager.OnBallExplode += this.OnBallExplode;
        this.playerPoints = 0;
    }

    private void OnBallExplode(GridCell[] arg0)
    {
        this.playerPoints += 10;
        this.text.text = playerPoints.ToString();
    }
}
