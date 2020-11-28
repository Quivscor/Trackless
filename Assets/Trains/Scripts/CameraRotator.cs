using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField]
    private float maxRotation = 10.0f;

    public void SetPercentageRotation(float percentageValue)
    {
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x, -1 * percentageValue * maxRotation / 100.0f, this.transform.localRotation.eulerAngles.z);
    }
}
