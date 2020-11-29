﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameCanvasController : MonoBehaviour
{
    public static EndGameCanvasController Instance = null;

    private void Awake()
    {
        if (EndGameCanvasController.Instance == null)
            EndGameCanvasController.Instance = this;
        else
            Destroy(this);
    }

    public void DisplayCanvas(bool value)
    {
        GetComponent<Canvas>().enabled = value;

        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public TMPro.TextMeshProUGUI pointsReached;
    public TMPro.TextMeshProUGUI pointsTotal;
    public TMPro.TextMeshProUGUI steelGained;
    public TMPro.TextMeshProUGUI hitTrees;

    public TracklessGenerator.MapGenerator mapGenerator = null;

    public void SetPointsReached(int value)
    {
        if (pointsReached)
            pointsReached.text = value + "";
    }

    public void SetPointsInTotal()
    {
        if (pointsTotal)
            pointsTotal.text = 0 + "";
    }

    public void SetSteelGained(int value)
    {
        if (steelGained)
            steelGained.text = value + "";
    }

    public void SetHitTrees(int value)
    {
        if (hitTrees)
            hitTrees.text = value + "";
    }

    public void LoadNewLevel()
    {
        mapGenerator?.ClearMap();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}