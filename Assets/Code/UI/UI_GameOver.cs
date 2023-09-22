using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{

    [SerializeField] GameObject viewObject;
    [SerializeField] TMPro.TextMeshProUGUI playerScoreText;
    [SerializeField] LevelManager levelManager;
    [SerializeField] PlayerManager playerManager;

    private void Awake()
    {
        levelManager.OnGameOver += this.OnGameOver;
    }

    private void OnGameOver()
    {
        this.playerScoreText.text = this.playerManager.PlayerScore.ToString();
        this.viewObject.SetActive(true);
    }

}
