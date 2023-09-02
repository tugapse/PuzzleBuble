using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{

    public bool SnapToGridX;
    public bool SnapToGridY;
    void OnDrawGizmosSelected() {
        Vector3 pos = GameGridHelper.getGridPosition( this.transform.position);
        float x = SnapToGridX ? pos.x : this.transform.position.x;
        float y = SnapToGridY ? pos.y : this.transform.position.y;
        this.transform.position = new Vector3(x, y,0);
    }
}
