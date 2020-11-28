using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityFOVChanger : MonoBehaviour
{
    public float maxVelocity;

    public float minFOV;
    public float maxFOV;

    private Cinemachine.CinemachineFreeLook cameraBrain;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraBrain = GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
    }

    private void Update()
    {
        cameraBrain.m_Lens.FieldOfView = Mathf.Lerp(minFOV, maxFOV, rb.velocity.magnitude/maxVelocity);
    }
}
