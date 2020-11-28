using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonRotator : MonoBehaviour
{
    [SerializeField]
    private float maxRotation = 10.0f;

    public GameObject leftSideSparks;
    public GameObject rightSideSparks;

    public void SetPercentageRotation(float percentageValue)
    {
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, percentageValue * maxRotation / 100.0f);

        leftSideSparks?.SetActive(this.transform.localRotation.eulerAngles.z > 8.0f && this.transform.localRotation.eulerAngles.z < 180.0f);
        rightSideSparks?.SetActive(this.transform.localRotation.eulerAngles.z < 352.0f && this.transform.localRotation.eulerAngles.z > 180.0f);
    }
}
