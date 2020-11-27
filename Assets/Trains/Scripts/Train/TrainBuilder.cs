using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBuilder : MonoBehaviour
{
    public static TrainBuilder Instance;

    private void Awake()
    {
        if (TrainBuilder.Instance == null)
            TrainBuilder.Instance = this;
        else
            Destroy(this);
    }

    public GameObject[] availableWagons;

    public void BuildBasicTrain(TrainManager trainManager)
    {
        GameObject wagonToCreate = GetWagonOfType(WagonType.Coal);
        if (wagonToCreate)
            trainManager.CreateWagon(wagonToCreate);

        wagonToCreate = GetWagonOfType(WagonType.Passengers);

        for (int i = 0; i < 2; i++)
        {
            if (wagonToCreate)
                trainManager.CreateWagon(wagonToCreate);
        }
    }

    public void BuildWagon(TrainManager trainManager, WagonType type)
    {

    }

    private GameObject GetWagonOfType(WagonType type)
    {
        foreach (GameObject wagon in availableWagons)
        {
            if (wagon.GetComponent<Wagon>().WagonType == type)
                return wagon;
        }

        return null;
    }
}
