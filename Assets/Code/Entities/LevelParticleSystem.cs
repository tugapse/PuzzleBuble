using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParticleSystem : MonoBehaviour
{

    [Header("Particle Systems")]
    [SerializeField] ParticleSystem ballExplosion;
    [SerializeField] ParticleSystem ballBottomExplosion;
    [SerializeField] ParticleSystemManager particleSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        particleSystemManager.OnBallExplosion += this.EmmitBallExplosionParticles;
        particleSystemManager.OnBallBottomExplosion += this.EmmitBallBottomExplosionParticles;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void EmmitBallExplosionParticles(Vector3 position, Color color, int count = 20)
    {
        ParticleSystem.MainModule settings = this.ballExplosion.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        this.ballExplosion.transform.position = position;
        this.ballExplosion.Emit(count);
    }
    void EmmitBallBottomExplosionParticles(Vector3 position, Color color, int count = 10)
    {
        ParticleSystem.MainModule settings = this.ballBottomExplosion.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        this.ballBottomExplosion.transform.position = position;
        this.ballBottomExplosion.Emit(count);
    }
}
