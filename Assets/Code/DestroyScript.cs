using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    [SerializeField] ParticleSystemManager particleSystemManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] PlayerManager playerManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            if (ball.parentCell == null) return;
            ball.parentCell.Clear();
            this.playerManager.AddScorePoints(20);
            this.levelManager.BallDestroy(Camera.main.WorldToScreenPoint(other.transform.position), ball.explosionColor);
            this.particleSystemManager.BallDestroyed(other.transform.position, other.GetComponent<Ball>().explosionColor);
        }
    }


}
