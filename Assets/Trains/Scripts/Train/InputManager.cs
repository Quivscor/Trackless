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
        Data.moveX = Input.GetAxisRaw("Horizontal");
        Data.moveY = Input.GetAxis("Vertical");

        //actions
        Data.cauldronBoost = Input.GetKeyDown(KeyCode.Space);
        Data.isBrake = Input.GetAxis("Vertical") < 0 ? true : false;
    }
}

public class InputData
{
    public float moveX;
    public float moveY;
    public bool isBrake;
    public bool cauldronBoost;
}
