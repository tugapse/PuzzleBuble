using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningStage : MonoBehaviour
{
    public LevelManager levelManager;

    private void OnTriggerStay2D(Collider2D other)
    {
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell == null || ball.parentCell.isFalling) return;
            this.levelManager.SetWarningState();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            this.levelManager.SetNormalState();

        }
    }



}
