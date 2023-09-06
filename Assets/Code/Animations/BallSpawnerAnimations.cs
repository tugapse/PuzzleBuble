using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BallSpawnerAnimations : MonoBehaviour
{

    public SpriteRenderer currentBall;
    public Animator animator;
    public PlayerControler playerControler;

    public void StartAnimation()
    {
        this.HideCurrentBall();
        animator.SetBool("swap", true);

    }

    public void StopAnimation()
    {
        animator.SetBool("swap", false);
        this.playerControler.canShoot = true;

    }
    public void ShowCurrentBall()
    {
        if (this.currentBall != null)
        {
            currentBall.GetComponent<SpriteRenderer>().enabled = true;
        }

    }
    public void HideCurrentBall()
    {
        if (this.currentBall != null)
        {
            currentBall.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

}
