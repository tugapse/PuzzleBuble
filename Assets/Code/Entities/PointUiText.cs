using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUiText : MonoBehaviour
{
    [SerializeField] float minSpeed = 2;
    [SerializeField] float maxSpeed = 4;
    [SerializeField] PlayerManager playerManager;

    public float delay = 0.8f;



    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.parent.position, UnityEngine.Random.Range(minSpeed, maxSpeed) * Time.deltaTime);
        if (Vector3.Distance(transform.position, transform.parent.position) < 120)
        {
            this.playerManager.AddScorePoints(10);
            Destroy(this.gameObject);
        }
    }
}
