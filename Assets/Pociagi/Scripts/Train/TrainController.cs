using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float accelerationForce;
    public float torqueForce;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = InputManager.Data.moveY * transform.forward;
        Vector3 torque = InputManager.Data.moveX * transform.up;

        if(velocity != Vector3.zero)
            rb.AddForce(velocity * accelerationForce);
        rb.AddTorque(torque * torqueForce);
    }
}
