using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    [SerializeField] ParticleSystemManager particleSystemManager;
    [SerializeField] LevelManager levelManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            ball.parentCell.Clear();
            this.levelManager.BallExplode(new List<GridCell> { ball.parentCell });
            this.particleSystemManager.BallBottomExplosion(other.transform.position, other.GetComponent<Ball>().ExplosionColor);
        }
    }

}
