using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SnowstormPropertiesAccessor : MonoBehaviour
{
    private static VisualEffect snowstormEffect;

    private void Awake()
    {
        snowstormEffect = GetComponent<VisualEffect>();
    }

    public static void IncreaseSnowStrength()
    {
        snowstormEffect.SetVector2("Range", new Vector2(5000, 10000));
    }

    public static void DecreaseSnowStrength()
    {
        snowstormEffect.SetVector2("Range", new Vector2(500, 1000));
    }
}
