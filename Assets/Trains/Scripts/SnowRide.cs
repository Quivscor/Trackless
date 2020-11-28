using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SnowRide : MonoBehaviour
{
    public VisualEffect visualEffect;

    private float minSpeed = 1.0f;
    private float maxSpeed = 10.0f;

    private int minParticles = 0;
    private int maxParticles = 300;

    private bool isOnIce = false;

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    public void SetPercentageEffectSpeed(float value)
    {
        visualEffect.playRate = value / 100.0f * (maxSpeed - minSpeed) + minSpeed;

        if (value < 1 || isOnIce)
            visualEffect.SetInt("Rate", minParticles);
        else
            visualEffect.SetInt("Rate", maxParticles);
    }

    public void SetIsOnIce(bool value)
    {
        isOnIce = value;
    }
}
