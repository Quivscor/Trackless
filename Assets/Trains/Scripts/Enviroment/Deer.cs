using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 10.0f;

    [SerializeField]
    private float idleMinTime = 3.0f;
    [SerializeField]
    private float idleMaxTime = 10.0f;

    [SerializeField]
    private float runningMinTime = 1.0f;
    [SerializeField]
    private float runningMaxTime = 2.0f;

    private bool isRunningAwayFromSth = false;
    private bool isRunning = false;
    public GameObject scaryThing = null;

    private float actionTimer = 0.0f;
    private float actionCurrentTimer = 0.0f;

    public void StartRunning(GameObject fromThis)
    {
        isRunningAwayFromSth = true;
        scaryThing = fromThis;
    }

    public void StopRunning()
    {
        isRunningAwayFromSth = false;
        actionCurrentTimer = 0.0f;
        actionTimer = Random.Range(idleMinTime, idleMaxTime);
        isRunning = false;
    }

    private void Start()
    {
        actionCurrentTimer = 0.0f;
        this.transform.Rotate(0, Random.Range(0, 360), 0);
        actionTimer = Random.Range(idleMinTime, idleMaxTime);
        isRunning = false;
    }

    private void Update()
    {
        actionCurrentTimer += Time.deltaTime;

        if (isRunningAwayFromSth)
        {
            this.transform.LookAt(scaryThing.transform.position);
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y * (-1), 0);
            this.transform.position += this.transform.forward * Time.deltaTime * runSpeed;
        }
        else if (actionCurrentTimer >= actionTimer)
        {
            actionCurrentTimer = 0.0f;
            this.transform.Rotate(0, Random.Range(0, 360), 0);

            if (isRunning)
            {
                actionTimer = Random.Range(idleMinTime, idleMaxTime);
                isRunning = false;
            }
            else
            {
                actionTimer = Random.Range(runningMinTime, runningMaxTime);
                isRunning = true;
            }
        }
        else if (isRunning)
        {
            this.transform.position += this.transform.forward * Time.deltaTime * runSpeed;
        }

    }
}
