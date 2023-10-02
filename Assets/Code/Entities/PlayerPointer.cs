using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPointer : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform playerPointer;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetPoints(playerPointer.TransformPoint(playerPointer.position),
         playerPointer.TransformDirection(playerPointer.transform.up), 10);
        Debug.Log(playerPointer.transform.up);
    }

    Vector3[] GetPoints(Vector2 origin, Vector2 direction, int maxIteration, int counter = 0)
    {
        if (counter == 0)
        {
            lineRenderer.SetPosition(counter, origin);
            counter++;
        }
        var hit = Physics2D.Raycast(origin, direction, 1000);
        if (hit.collider != null)
        {
            var point = transform.InverseTransformPoint(hit.point);
            lineRenderer.SetPosition(counter, point);
        }
        return null;
    }
}
