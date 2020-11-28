using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrainController>() || other.GetComponent<Deer>())
        {
            animator.enabled = true;
            animator.ResetTrigger("shakeTree");
            animator.SetTrigger("shakeTree");
        }

    }

    public void TurnOffAnimator()
    {
        animator.ResetTrigger("shakeTree");
        animator.enabled = false;
    }
}
