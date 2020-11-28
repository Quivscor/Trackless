using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wagon))]
public class TrainManager : MonoBehaviour
{
    private List<Wagon> wagons = new List<Wagon>();

    private void Start()
    {
        wagons.Add(GetComponent<Wagon>());

        //TrainBuilder.Instance.BuildBasicTrain(this);
    }

    public void SetWheelsRotationSpeed(float percentageSpeed)
    {
        foreach (Wagon wagon in wagons)
            wagon?.SetPercentageSpeed(percentageSpeed);
    }

    public void CreateWagon(GameObject wagon)
    {
        Wagon lastWagon = wagons[wagons.Count - 1];

        GameObject newWagon = Instantiate(wagon);
        newWagon.transform.position = lastWagon.jointConnectorEnd.transform.position;
        newWagon.transform.position -= newWagon.GetComponent<Wagon>().jointConnectorStart.transform.localPosition;
        newWagon.transform.rotation = lastWagon.transform.rotation;

        newWagon.GetComponent<HingeJoint>().connectedBody = lastWagon.jointConnectorEnd.GetComponent<Rigidbody>();

        wagons.Add(newWagon.GetComponent<Wagon>());
    }
}
