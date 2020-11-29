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
        if (other.GetComponent<TrainController>())
        {
            //if (other.GetComponent<Inventory>().Passengers < uiDataManager.mapGenerator.numberOfPassengers)
            //    uiDataManager.TurnOnEndGameText(true, "Find other passengers!");
            //else
            //{
            //    uiDataManager.TurnOnEndGameText(true, "Congratulations!");
            //    Time.timeScale = 0;
            //}
            FindObjectOfType<TracklessGenerator.MapGenerator>().TurnOffCollidersOfEndPointNeighbours();
            FindObjectOfType<IntroAndOutroManager>().PlayOutroSequence();
            StartCoroutine(FindObjectOfType<UIDataManager>().FadeToBlack(5.5f));

            SetCanvasValues();
        }
    }

    private void SetCanvasValues()
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        EndGameCanvasController.Instance.SetPointsReached(inventory.GetPoints());
        EndGameCanvasController.Instance.SetSteelGained(inventory.Steel);
        EndGameCanvasController.Instance.SetHitTrees();
        EndGameCanvasController.Instance.SetTime();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TrainController>())
            uiDataManager.TurnOnEndGameText(false, "None!");
    }

    public void GameOverLostHeat()
    {
        Debug.Log("Lost to heat!");
    }
}
