﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrainManager))]
public class TrainController : MonoBehaviour
{
    private TrainMovement trainMovement = TrainMovement.Stopped;
    private TrainManager trainManager;
    private CameraRotator cameraRotator;
    private SnowRide snowRide;

    public const float maxAngularVelocityForRotation = 0.6f;

    public float reverseMaxVelocity;
    public float minVelocity;
    public float maxVelocity;
    public float torqueForce;

    public float maxAngularVelocity = 1.5f;

    public float angularSmoothingTime;
    private Vector3 angularSmoothing;
    public float minVelocityToRotate;

    public float maxAccelerationTime = 3f;
    private float currentAccelerationTime;

    private float reverseAccelerationTime;
    public float maxReverseAccelerationTime = 1f;

    public float wallHitVelocityThreshold;

    [Header("Ice")]
    public LayerMask iceMask;
    private bool isOnIce = false;
    private Vector3 lastFrameAngularVelocity;
    public float iceDecelerationMultiplier;
    public float iceSlideForceMultiplier;
    private float iceTorqueTime;
    public float maxIceTorqueTime;
    private float iceTorqueDirection;
    public float iceTorqueTimeInitialValue;

    private Rigidbody rb;
    private BoxCollider collider;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();

        rb.maxAngularVelocity = maxAngularVelocity;

        trainManager = GetComponent<TrainManager>();

        cameraRotator = GetComponentInChildren<CameraRotator>();
        snowRide = GetComponentInChildren<SnowRide>();
    }

    private void FixedUpdate()
    {
        isOnIce = DetectIce();

        ForwardMovement();
        Turning();
    }

    private void ForwardMovement()
    {
        float additionalMovement = isOnIce ? iceDecelerationMultiplier : 1f;

        SetTrainState();
        //if (InputManager.Data.moveY > 0)
        //{
        //    trainMovement = TrainMovement.Accelerating;
        //    currentAccelerationTime = currentAccelerationTime >= maxAccelerationTime ? maxAccelerationTime : currentAccelerationTime + (Time.fixedDeltaTime * iceDecelerationMultiplier);
        //}
        //else if (currentAccelerationTime > 0)
        //{
        //    trainMovement = TrainMovement.Decelerating;
        //    currentAccelerationTime = currentAccelerationTime <= 0 ? 0 : currentAccelerationTime - (Time.fixedDeltaTime / iceDecelerationMultiplier);
        //}
        //else if (InputManager.Data.moveY == 0)
        //{
        //    trainMovement = TrainMovement.Stopped;
        //}

        if(trainMovement != TrainMovement.Reversing)
        {
            rb.velocity = Vector3.Lerp(minVelocity * transform.forward, maxVelocity * transform.forward, (currentAccelerationTime / maxAccelerationTime));
        }
        else
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, reverseMaxVelocity * transform.forward * -1, (reverseAccelerationTime / maxReverseAccelerationTime));
        }

        trainManager.SetWheelsRotationSpeed((currentAccelerationTime < 0 ? 0 : currentAccelerationTime) / maxAccelerationTime * 100);
        snowRide.SetPercentageEffectSpeed((currentAccelerationTime < 0 ? 0 : currentAccelerationTime) / maxAccelerationTime * 100);
        snowRide.SetIsOnIce(isOnIce);
    }

    private void SetTrainState()
    {
        switch (trainMovement)
        {
            case TrainMovement.Accelerating:
                {
                    if (InputManager.Data.moveY > 0)
                    {
                        currentAccelerationTime = currentAccelerationTime >= maxAccelerationTime ?
                            maxAccelerationTime :
                            currentAccelerationTime + (Time.fixedDeltaTime * iceDecelerationMultiplier);
                    }
                    else if (currentAccelerationTime >= 0)
                    {
                        trainMovement = TrainMovement.Decelerating;
                    }
                    break;
                }
            case TrainMovement.Decelerating:
                {
                    if (InputManager.Data.moveY > 0)
                    {
                        trainMovement = TrainMovement.Accelerating;
                    }
                    if (currentAccelerationTime > 0)
                    {
                        currentAccelerationTime = currentAccelerationTime <= 0 ? 0 : currentAccelerationTime - (Time.fixedDeltaTime / iceDecelerationMultiplier);
                        if (rb.velocity.magnitude < wallHitVelocityThreshold)
                            currentAccelerationTime = 0;
                    }
                    else
                        trainMovement = TrainMovement.Stopped;
                    break;
                }
            case TrainMovement.Stopped:
                {
                    if (InputManager.Data.moveY > 0)
                    {
                        trainMovement = TrainMovement.Accelerating;
                    }
                    else if (InputManager.Data.moveY < 0)
                    {
                        trainMovement = TrainMovement.Reversing;
                    }
                    break;
                }
            case TrainMovement.Reversing:
                {
                    if (InputManager.Data.moveY < 0)
                    {
                        reverseAccelerationTime = reverseAccelerationTime >= maxReverseAccelerationTime ?
                            maxReverseAccelerationTime :
                            reverseAccelerationTime + Time.fixedDeltaTime;
                    }
                    else if (reverseAccelerationTime > 0)
                    {
                        reverseAccelerationTime = reverseAccelerationTime <= 0 ?
                            0 :
                            reverseAccelerationTime - Time.fixedDeltaTime * 2;
                    }
                    else
                        trainMovement = TrainMovement.Stopped;
                    break;
                }
        }
    }

    private void Turning()
    {
        if (trainMovement != TrainMovement.Stopped && Vector3.Magnitude(rb.velocity) > minVelocityToRotate)
        {
            Vector3 torqueDirection = new Vector3(0, InputManager.Data.moveX != 0 ? Mathf.Sign(InputManager.Data.moveX) : 0, 0);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity,
               torqueDirection * (torqueForce * (currentAccelerationTime / maxAccelerationTime)) * Time.fixedDeltaTime,
               angularSmoothingTime);

            TurningOnIce();

        }
        else
            rb.angularVelocity = Vector3.zero;

        trainManager.SetWagonsRotation(rb.angularVelocity.y / maxAngularVelocityForRotation * 100.0f);
        cameraRotator.SetPercentageRotation(rb.angularVelocity.y / maxAngularVelocityForRotation * 100.0f);
    }

    private void TurningOnIce()
    {
        //Ice part
        if (isOnIce)
        {
            if (iceTorqueTime == 0)
            {
                iceTorqueDirection = Mathf.Sign(InputManager.Data.moveX);
                iceTorqueTime += iceTorqueTimeInitialValue;
            }

            if (iceTorqueDirection == Mathf.Sign(InputManager.Data.moveX) && InputManager.Data.moveX != 0)
                iceTorqueTime = iceTorqueTime >= maxIceTorqueTime ? maxIceTorqueTime : iceTorqueTime + Time.fixedDeltaTime;
            else
                iceTorqueTime = iceTorqueTime <= 0 ? 0 : iceTorqueTime - Time.fixedDeltaTime;

            rb.angularVelocity += transform.up * iceTorqueDirection * iceSlideForceMultiplier * (iceTorqueTime / maxIceTorqueTime);
        }
        else
            iceTorqueTime = 0;
    }

    private bool DetectIce()
    {
        return Physics.Raycast(transform.position, Vector3.down, 5.0f, iceMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 5.0f);
    }
}

public enum TrainMovement
{
    Stopped,
    Accelerating,
    Decelerating,
    Reversing
}
