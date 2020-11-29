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
        foreach (WagonType wagonType in TrainDataContainer.Instance.wagonsInTrain)
        {
            GameObject wagonToCreate = GetWagonOfType(wagonType);
            if (wagonToCreate)
                trainManager.CreateWagon(wagonToCreate);
        }
    }

    public void BuildWagon(TrainManager trainManager, WagonType type)
    {
        GameObject wagonToCreate = GetWagonOfType(type);
        if (wagonToCreate)
            trainManager.CreateWagon(wagonToCreate);
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
