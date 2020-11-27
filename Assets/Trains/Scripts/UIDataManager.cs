using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDataManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private Image heatMeter;

    private Cauldron cauldron;

    void Awake()
    {
        FindObjectOfType<TracklessGenerator.MapGenerator>().action += FindCauldron;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeatMeter();
    }

    private void FindCauldron()
    {
        cauldron = FindObjectOfType<Cauldron>();
    }

    private void UpdateHeatMeter()
    {
        heatMeter.fillAmount = cauldron.CurrentCauldronLevel / cauldron.CauldronMaxLevel;
    }
}
