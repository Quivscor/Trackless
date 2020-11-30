using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputData Data { get; private set; }

    bool oldControls = false;
    void Awake()
    {
        Data = new InputData();
       
    }
    private void Start()
    {

        if (PlayerPrefs.HasKey("Controls"))
        {
            if (PlayerPrefs.GetInt("Controls") == 1)
                oldControls = true;
        }
    }

    void Update()
    {
        //movement
        Data.moveX = Input.GetAxisRaw("Horizontal");
        Data.moveY = Input.GetAxis("Vertical");

        if (Data.moveY == 0)
            Data.moveY = 1;

        //actions
        if(!oldControls)
        {
            Data.cauldronBoost = Input.GetKeyDown(KeyCode.LeftShift);
            //Data.isBrake = Input.GetAxis("Vertical") < 0 ? true : false;

            Data.isBrake = Input.GetKey(KeyCode.Space);
        }
        else
        {
            Data.cauldronBoost = Input.GetKeyDown(KeyCode.Space);
            //Data.isBrake = Input.GetAxis("Vertical") < 0 ? true : false;

            Data.isBrake = Input.GetKey(KeyCode.LeftAlt);
        }
       

        //if (Input.GetKeyDown(KeyCode.T))
        //    TrainBuilder.Instance.BuildBasicTrain(this.GetComponent<TrainManager>());

        //Data.clear = Input.GetKey(KeyCode.F5);

    }
}

public class InputData
{
    public float moveX;
    public float moveY;
    public bool isBrake;
    public bool cauldronBoost;
    public bool clear;
    public bool oldControls;
}
