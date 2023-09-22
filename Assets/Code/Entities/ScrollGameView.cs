using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolllGameView : MonoBehaviour
{
    [SerializeField] float Speed = -0.02f;
    [SerializeField] LevelManager levelManager;


    // Update is called once per frame
    void Update()
    {
        if (levelManager?.LevelRunning == false) return;
        this.transform.position = transform.position + Vector3.down * (Time.deltaTime * Speed);

    }
}
