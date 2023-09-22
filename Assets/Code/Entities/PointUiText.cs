using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUiText : MonoBehaviour
{
    [SerializeField] float minSpeed = 2;
    [SerializeField] float maxSpeed = 4;
    [SerializeField] float minDistance = 120;
    [SerializeField] PlayerManager playerManager;

    public float delay = 0.8f;

    void Start()
    {
        StartCoroutine(this.GoToTarget());
    }
    IEnumerator GoToTarget()
    {

        yield return new WaitForSeconds(delay);

        while (Vector3.Distance(transform.position, transform.parent.position) > minDistance)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.transform.parent.position, UnityEngine.Random.Range(minSpeed, maxSpeed) * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        this.playerManager.AddScorePoints(10);
        Destroy(this.gameObject);
    }

}
