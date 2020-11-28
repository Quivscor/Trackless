using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameController : MonoBehaviour
{

    private UIDataManager uiDataManager;

    private void Start()
    {
        uiDataManager = FindObjectOfType<UIDataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Inventory>().Passengers < uiDataManager.mapGenerator.numberOfPassengers)
            uiDataManager.TurnOnEndGameText(true, "Find other passengers!");
        else
            uiDataManager.TurnOnEndGameText(true, "Congratulations!");

    }
    private void OnTriggerExit(Collider other)
    {
        uiDataManager.TurnOnEndGameText(false, "None!");
    }
}
