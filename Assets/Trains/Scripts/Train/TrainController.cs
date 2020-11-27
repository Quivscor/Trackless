using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float accelerationForce;
    public float torqueForce;

    public float maxAngularVelocity = 1.5f;
    public float maxVelocity = 5f;

    private Vector3 forwardSmoothing;
    private Vector3 angularSmoothing;
    public float forwardSmoothingTime;
    public float angularSmoothingTime;
    public float brakingSmoothingTime;

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

        if(InputManager.Data.isBrake)
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakingSmoothingTime);
        else if(velocity != Vector3.zero)
            rb.velocity = Vector3.Lerp(rb.velocity, velocity * accelerationForce, forwardSmoothingTime);


        if (velocity != Vector3.zero)
        {
            Vector3 torqueDirection = new Vector3(0, InputManager.Data.moveX != 0 ? Mathf.Sign(InputManager.Data.moveX) : 0, 0);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, torqueDirection * torqueForce * Time.fixedDeltaTime, angularSmoothingTime);
            //rb.angularVelocity = Vector3.SmoothDamp(transform.rotation.eulerAngles, torqueDirection * torqueForce, ref angularSmoothing, angularSmoothingTime);
        }
    }
}
