using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Database/Level Particle System")]
public class ParticleSystemManager : ScriptableObject
{
    public UnityAction<Vector3, Color, int> OnBallExplosion;
    public UnityAction<Vector3, Color, int> OnBallBottomExplosion;

    public void BallExplosion(Vector3 position, Color color, int count = 20)
    {
        OnBallExplosion?.Invoke(position, color, count);
    }
    public void BallBottomExplosion(Vector3 position, Color color, int count = 10)
    {
        OnBallExplosion?.Invoke(position, color, count);
    }
}
