using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wagon))]
public class TrainManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;
    private List<Wagon> wagons = new List<Wagon>();
    private List<WagonType> staticWagons = new List<WagonType>();

    private void Start()
    {
        wagons.Add(GetComponent<Wagon>());

        TrainBuilder.Instance.BuildBasicTrain(this);
    }

    public void SetWheelsRotationSpeed(float percentageSpeed)
    {
        foreach (Wagon wagon in wagons)
            wagon?.SetPercentageSpeed(percentageSpeed);
    }

    public void SetWagonsRotation(float percentageRotation)
    {
        foreach (Wagon wagon in wagons)
            wagon?.WagonRotator.SetPercentageRotation(percentageRotation);
    }

    public void CreateWagon(GameObject wagon)
    {
        Wagon lastWagon = wagons[wagons.Count - 1];

        GameObject newWagon = Instantiate(wagon, lastWagon.transform.position - new Vector3(0, -50.0f, 0), lastWagon.transform.rotation);
        newWagon.transform.rotation = lastWagon.transform.rotation;
        newWagon.transform.position = lastWagon.jointConnectorEnd.transform.position - (newWagon.GetComponent<Wagon>().jointConnectorStart.transform.position - newWagon.transform.position);
        newWagon.GetComponent<HingeJoint>().connectedBody = lastWagon.jointConnectorEnd.GetComponent<Rigidbody>();

        wagons.Add(newWagon.GetComponent<Wagon>());
        audio.Play();
    }
}
