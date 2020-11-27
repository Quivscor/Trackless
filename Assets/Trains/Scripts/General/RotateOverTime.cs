using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1.0f;

    [SerializeField]
    private Vector3 rotationVector = Vector3.zero;

    private void Update()
    {
        this.transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }
}
