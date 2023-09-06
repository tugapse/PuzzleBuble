using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    public Transform target;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }
    private bool checkingRay = false;

    // Update is called once per frame
    void Update()
    {
        if (checkingRay) return;
        checkingRay = true;
        var points = this.GetPoints(transform.position, transform.up, 0, 10);
        if (points == null) return;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        checkingRay = false;

    }

    List<Vector3> GetPoints(Vector3 from, Vector3 direction, int count, int limit, List<Vector3> points = null)
    {
        if (count > limit) return points;
        if (points == null) points = new List<Vector3>();
        RaycastHit2D hit = Physics2D.Raycast(from, direction, 100);
        if (hit.collider != null)
        {
            points.Add(hit.point);
            if (hit.collider.gameObject.tag != "Ball")
            {
                count++;
                float toX = Mathf.Abs(direction.x);
                points.AddRange(GetPoints(hit.point, new Vector3(direction.x < 0 ? -toX : toX, direction.y, 0), count, limit, points));
            }
            else
            {
            }
            target.transform.position = hit.point;
        }


        return points;
    }

    private void OnDrawGizmos()
    {



    }

}
