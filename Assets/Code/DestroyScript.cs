using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    private bool timerActive = false;
    private float waitTime = 5;
    public void StartTimer()
    {
        this.timerActive = true;

    }
    private void Update()
    {
        if (!timerActive) return;

        waitTime -= Time.deltaTime;

        if (waitTime < 0)
        {
            waitTime = 0;
            Destroy(gameObject);
        }

    }
}
