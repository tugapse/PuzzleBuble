using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject balls;
    Rect bounds;

    // Start is called before the first frame update
    void Start()
    {
        this.bounds = new Rect(this.transform.position + new Vector3(-this.transform.localScale.x / 2, -this.transform.localScale.y / 2, 0), this.transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
