using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDataContainer : MonoBehaviour
{
    public static TrainDataContainer Instance = null;

    private void Awake()
    {
        if (TrainDataContainer.Instance == null)
            TrainDataContainer.Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public List<WagonType> wagonsInTrain = new List<WagonType>();
}
