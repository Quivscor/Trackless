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

        DisplayCanvas(false);
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
    public TMPro.TextMeshProUGUI time;
    public TMPro.TextMeshProUGUI steelGained;
    public TMPro.TextMeshProUGUI hitTrees;

    public TracklessGenerator.MapGenerator mapGenerator = null;

    public void SetPointsReached(int value)
    {
        if (pointsReached)
            pointsReached.text = value + "";

        SetPointsInTotal(value);
    }

    private void SetPointsInTotal(int currentPoints)
    {
        int points = 0;
        int steel = 0;

        if (pointsTotal)
        {
            if (PlayerPrefs.HasKey("totalPoints"))
            {
                points = currentPoints + PlayerPrefs.GetInt("totalPoints");
            }
            else
            {
                points = currentPoints;
            }

            pointsTotal.text = points + "";
        }

        if (PlayerPrefs.HasKey("steel"))
        {
            steel = FindObjectOfType<Inventory>().Steel + PlayerPrefs.GetInt("steel");
        }
        else
        {
            steel = FindObjectOfType<Inventory>().Steel;
        }

        if (PlayerPrefs.HasKey("mapSize"))
        {
            PlayerPrefs.SetInt("mapSize", PlayerPrefs.GetInt("mapSize") + 10);
            
        }
        else
        {
            PlayerPrefs.SetInt("mapSize", FindObjectOfType<TracklessGenerator.MapGenerator>().mapSize + 10);
        }

        PlayerPrefs.SetInt("steel", steel);
        PlayerPrefs.SetInt("totalPoints", points);
        PlayerPrefs.Save();
    }

    public void SetTime()
    {
        if (time)
            time.text = TimeCounter.GetTime();
    }

    public void SetSteelGained(int value)
    {
        if (steelGained)
            steelGained.text = value + "";
    }

    public void SetHitTrees()
    {
        if (hitTrees)
            hitTrees.text = Inventory.GetHitTrees() + "";
    }

    public void LoadNewLevel()
    {
        SceneManager.LoadScene("Map Generating");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
