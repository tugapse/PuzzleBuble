using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Database/Player")]
public class PlayerManager : ScriptableObject
{
    public float shootForce;
    public float RotationSpeed;
    public float maxRotationAngle;

    public UnityAction<Vector3> OnShoot;
    public UnityAction<float> OnTurn;
    public UnityAction<int> OnScorePointsChanged;

    public void Shoot(Vector3 direction)
    {
        if (this.OnShoot != null) this.OnShoot(direction);
    }

    public void Turn(float angle)
    {
        if (this.OnShoot != null) this.OnTurn(angle);
    }

    public void AddScorePoints(int points)
    {
        this.OnScorePointsChanged?.Invoke(points);
    }

}
