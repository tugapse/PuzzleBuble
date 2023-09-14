using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public ParticleSystem explotionParticles;
    public LevelManager levelManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            ball.parentCell.Clear();
            this.levelManager.BallExplode(new List<GridCell> { ball.parentCell });
            this.EmmitExplosionParticles(other.transform.position, other.GetComponent<Ball>().ExplosionColor);
        }
    }

    private void EmmitExplosionParticles(Vector3 position, Color color)
    {
        ParticleSystem.MainModule settings = this.explotionParticles.main;

        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        this.explotionParticles.transform.position = position;
        this.explotionParticles.Emit(20);
    }
}
