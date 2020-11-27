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

        if(velocity != Vector3.zero)
            rb.AddForce(velocity * accelerationForce);

        if (rb.velocity != Vector3.zero)
        {
            float torqueDirection = InputManager.Data.moveX != 0 ? Mathf.Sign(InputManager.Data.moveX) : 0;
            rb.angularVelocity = new Vector3(0, torqueDirection * torqueForce * Time.fixedDeltaTime, 0);
        }
    }
}
