using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    public bool run;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void MovePlayer()
    {
        if (!run)
        {
            animator?.SetBool("Run", true);
            run = true;
        }
    }

    public void StopPlayer()
    {
        if (run)
        {
            animator?.SetBool("Run", false);
            run = false;
        }
    }
    public void PlayerDead()
    {
        if (Random.Range(0, 100) > 50)
            animator?.SetTrigger("Dead1");
        else
            animator?.SetTrigger("Dead2");
    }
}
