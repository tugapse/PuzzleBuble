using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            if (ball != null && ball.parentCell != null && ball.parentCell.isFalling == false)
            {
                levelManager.GameOver();


            }
        }
    }

}
