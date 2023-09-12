using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningStage : MonoBehaviour
{
    public LevelManager levelManager;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            Debug.Log("Other Game Object " + other.gameObject.name);
            this.levelManager.WarningState();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            Debug.Log("Other Game Object " + other.gameObject.name);
            this.levelManager.NormalState();

        }
    }



}
