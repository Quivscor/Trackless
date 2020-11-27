using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Wagon : MonoBehaviour
{
    public GameObject jointConnectorStart;
    public GameObject jointConnectorEnd;

    [SerializeField]
    private float maxAnimationSpeed = 3.0f;

    [SerializeField]
    private WagonType wagonType;

    private Animator animator;

    public WagonType WagonType { get => wagonType; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPercentageSpeed(float speedPercent)
    {
        animator.speed = maxAnimationSpeed * speedPercent / 100.0f;
    }
}
