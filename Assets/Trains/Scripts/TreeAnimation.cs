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
        if(other.GetComponent<TrainController>() || other.GetComponent<Deer>())
        {
            animator.enabled = true;
            animator.Play("tree_shake");
        }
       
    }

    public void TurnOffAnimator()
    {
        animator.enabled = false;
    }
}
