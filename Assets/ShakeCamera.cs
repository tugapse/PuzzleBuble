using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Animator animator;
    public void StartShake()
    {
        animator.SetBool("shake", true);
    }

    public void StopShake()
    {
        animator.SetBool("shake", false);

    }
}
