using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    private TrainMovement trainMovement = TrainMovement.Stopped;

    public float maxVelocity;
    public float torqueForce;

    public float maxAngularVelocity = 1.5f;

    public float angularSmoothingTime;
    public float minVelocityToRotate;

    public float maxAccelerationTime = 3f;
    private float currentAccelerationTime;

    private bool isOnIce = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = maxAngularVelocity;
    }

    private void FixedUpdate()
    {
        ForwardMovement();

        //turning
        if (trainMovement != TrainMovement.Stopped && Vector3.Magnitude(rb.velocity) > minVelocityToRotate)
        {
            Vector3 torqueDirection = new Vector3(0, InputManager.Data.moveX != 0 ? Mathf.Sign(InputManager.Data.moveX) : 0, 0);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, torqueDirection * torqueForce * Time.fixedDeltaTime, rb.velocity.magnitude * angularSmoothingTime);
            //rb.angularVelocity = Vector3.SmoothDamp(transform.rotation.eulerAngles, torqueDirection * torqueForce, ref angularSmoothing, angularSmoothingTime);
        }
        else
            rb.angularVelocity = Vector3.zero;
    }

    private void ForwardMovement()
    {
        if(InputManager.Data.moveY > 0)
        {
            trainMovement = TrainMovement.Accelerating;
            currentAccelerationTime = currentAccelerationTime >= maxAccelerationTime ? maxAccelerationTime : currentAccelerationTime + Time.fixedDeltaTime;
        }
        else if(currentAccelerationTime > 0)
        {
            trainMovement = TrainMovement.Decelerating;
            currentAccelerationTime = currentAccelerationTime <= 0 ? 0 : currentAccelerationTime - Time.fixedDeltaTime;
        }
        else
        {
            trainMovement = TrainMovement.Stopped;
        }

        rb.velocity = Vector3.Lerp(Vector3.zero, maxVelocity * transform.forward, (currentAccelerationTime / maxAccelerationTime));
    }
}

public enum TrainMovement
{
    Stopped,
    Accelerating,
    Decelerating
}
