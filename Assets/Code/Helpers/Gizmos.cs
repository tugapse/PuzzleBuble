using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GizmoType
{
    Sphere, Bounds
}
public class Gizmos : MonoBehaviour
{
    public Color color;
    public GizmoType Shape;
    public Vector3 Size = new Vector3(1, 1, 1);
    public bool allwaysDrawGizmos = false;

    void OnDrawGizmos()
    {
        if (allwaysDrawGizmos) this.OnDrawGizmosSelected();
    }
    void OnDrawGizmosSelected()
    {
        UnityEngine.Gizmos.color = this.color;


        if (this.Shape == GizmoType.Bounds)
        {
            float left = transform.position.x - transform.localScale.x / 2;
            float top = transform.position.y + transform.localScale.y / 2;
            float right = transform.position.x + transform.localScale.x / 2;
            float bottom = transform.position.y - transform.localScale.y / 2;

            UnityEngine.Gizmos.DrawLine(new Vector3(left, top, 0), new Vector3(right, bottom, 0));
            UnityEngine.Gizmos.DrawLine(new Vector3(right, top, 0), new Vector3(left, bottom, 0));

            UnityEngine.Gizmos.DrawLine(new Vector3(left, top, 0), new Vector3(right, top, 0));
            UnityEngine.Gizmos.DrawLine(new Vector3(right, top, 0), new Vector3(right, bottom, 0));
            UnityEngine.Gizmos.DrawLine(new Vector3(right, bottom, 0), new Vector3(left, bottom, 0));
            UnityEngine.Gizmos.DrawLine(new Vector3(left, bottom, 0), new Vector3(left, top, 0));
        }
        else if (this.Shape == GizmoType.Sphere)
        {
            UnityEngine.Gizmos.DrawSphere(transform.position, this.Size.x);
        }
    }


}
