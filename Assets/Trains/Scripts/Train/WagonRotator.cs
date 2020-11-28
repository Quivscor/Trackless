using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonRotator : MonoBehaviour
{
    [SerializeField]
    private float maxRotation = 10.0f;

    public void SetPercentageRotation(float percentageValue)
    {
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, percentageValue * maxRotation / 100.0f);
    }
}
