using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gizmos : MonoBehaviour
{
    public Color color;
    public int Shape;
    public Vector3 Size = new Vector3(1, 1, 1);

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = this.color;


        if (this.Shape == 1)
        {
            Gizmos.DrawCube(transform.position, this.Size);
        }
        else if (this.Shape == 2)
        {
            Gizmos.DrawSphere(transform.position, this.Size.x);
        }
    }


}
