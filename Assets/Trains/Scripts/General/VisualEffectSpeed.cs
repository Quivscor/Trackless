using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectSpeed : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public VisualEffect visualEffect;

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        visualEffect.playRate = speed;
    }
}
