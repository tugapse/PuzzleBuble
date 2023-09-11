using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningStage : MonoBehaviour
{
    public GridData gridData;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell != null) this.gridData.WarningState();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.parentCell != null) this.gridData.NormalState();
        }
    }
}
