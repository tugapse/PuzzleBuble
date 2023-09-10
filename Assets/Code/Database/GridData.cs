using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Database/Grid Level")]
public class GridData : ScriptableObject
{

    public UnityAction<Collider2D[]> OnBallCollision;
    public UnityAction<GridCell[]> OnBallExplode;

    public void BallCollision()
    {
        if (this.OnBallCollision != null) this.OnBallCollision(null);
    }


    public void BallExplode(List<GridCell> balls)
    {
        if (this.OnBallExplode != null) this.OnBallExplode(balls.ToArray());
    }

}
