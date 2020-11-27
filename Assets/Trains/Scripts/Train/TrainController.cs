using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float accelerationForce;
    public float torqueForce;

    public float maxAngularVelocity = 1.5f;
    public float maxVelocity = 5f;

    private bool isOnIce = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = maxAngularVelocity;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = InputManager.Data.moveY > 0 ? InputManager.Data.moveY * transform.forward : Vector3.zero;
        Vector3 torque = Vector3.zero;

        if(velocity != Vector3.zero)
            rb.AddRelativeForce(velocity * accelerationForce);

        if (rb.velocity != Vector3.zero)
        {
            torque = InputManager.Data.moveX * transform.up;
            rb.AddRelativeTorque(torque * torqueForce);
        }
    }
}
