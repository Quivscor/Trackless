using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputData Data { get; private set; }
    void Awake()
    {
        Data = new InputData();
    }

    void Update()
    {
        //movement
        Data.moveX = Input.GetAxis("Horizontal");
        Data.moveY = Input.GetAxis("Vertical");

        //actions
        Data.isBrake = Input.GetKey(KeyCode.Space);
    }
}

public class InputData
{
    public float moveX;
    public float moveY;
    public bool isBrake;
}
