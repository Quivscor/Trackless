using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongSnowEnabler : MonoBehaviour
{
    public LayerMask locomotiveMask;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == (other.gameObject.layer | (1 << locomotiveMask)))
        {
            SnowstormPropertiesAccessor.IncreaseSnowStrength();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (other.gameObject.layer | (1 << locomotiveMask)))
        {
            SnowstormPropertiesAccessor.DecreaseSnowStrength();
        }
    }
}
