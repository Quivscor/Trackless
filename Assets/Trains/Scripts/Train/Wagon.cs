using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public GameObject jointConnectorStart;
    public GameObject jointConnectorEnd;

    [SerializeField]
    private float maxAnimationSpeed = 3.0f;

    [SerializeField]
    private WagonType wagonType;

    private Animator animator;

    private WagonRotator wagonRotator;

    public WagonType WagonType { get => wagonType; }

    public WagonRotator WagonRotator { get => wagonRotator; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        wagonRotator = GetComponentInChildren<WagonRotator>();
    }

    public void SetPercentageSpeed(float speedPercent)
    {
        animator.speed = maxAnimationSpeed * speedPercent / 100.0f;
    }
}
