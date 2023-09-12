using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningStage : MonoBehaviour
{
    public GridData gridData;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            Debug.Log("Other Game Object " + other.gameObject.name);
            this.gridData.WarningState();

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
            this.gridData.NormalState();

        }
    }



}
