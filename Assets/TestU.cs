using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestU : MonoBehaviour
{
    public float Speed = -0.02f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = transform.position + Vector3.down * (Time.deltaTime * Speed);

    }
}
