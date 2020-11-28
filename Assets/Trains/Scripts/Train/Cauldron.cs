using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public Inventory inventory = null;

    [SerializeField]
    private float burningSpeed = 1.0f;

    [SerializeField]
    private float burningBoostPerCoal = 20.0f;

    [SerializeField]
    private float cauldronMaxLevel = 150.0f;

    private float currentCauldronLevel = 0.0f;

    public float CauldronMaxLevel { get => cauldronMaxLevel; }
    public float CurrentCauldronLevel { get => currentCauldronLevel; }

    public const float HeatStatusPercentageLowLimit = 0.0f;
    public const float HeatStatusPercentageMediumLimit = 15.0f;
    public const float HeatStatusPercentageHighLimit = 50.0f;
    public const float HeatStatusPercentageOverheatedLimit = 80.0f;

    public HeatStatus HeatStatus { get; private set; }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        currentCauldronLevel = (HeatStatusPercentageOverheatedLimit / 100.0f) * cauldronMaxLevel;
        SetHeatStatus();
    }

    private void Update()
    {
        if (InputManager.Data.cauldronBoost)
            AddCoalToCauldron();

        currentCauldronLevel -= burningSpeed * Time.deltaTime;

        if (currentCauldronLevel < 0)
            currentCauldronLevel = 0;

        SetHeatStatus();
    }

    public void SetHeatStatus()
    {
        float percentageCauldronLevel = currentCauldronLevel / cauldronMaxLevel * 100.0f;

        if (percentageCauldronLevel >= HeatStatusPercentageOverheatedLimit)
            HeatStatus = HeatStatus.Overheated;
        else if (percentageCauldronLevel >= HeatStatusPercentageHighLimit)
            HeatStatus = HeatStatus.High;
        else if (percentageCauldronLevel >= HeatStatusPercentageMediumLimit)
            HeatStatus = HeatStatus.Medium;
        else if (percentageCauldronLevel > HeatStatusPercentageLowLimit)
            HeatStatus = HeatStatus.Low;
        else
            HeatStatus = HeatStatus.Faded;

    }

    private void IncreaseCauldronLevel()
    {
        currentCauldronLevel += burningBoostPerCoal;

        if (currentCauldronLevel > cauldronMaxLevel)
            currentCauldronLevel = cauldronMaxLevel;
    }

    private void AddCoalToCauldron()
    {
        if (inventory)
        {
            if (inventory.Coal > 0)
            {
                inventory.SpendCoal(1);

                IncreaseCauldronLevel();
            }
        }
    }

    public void SubtractHeatLevel(float value)
    {
        currentCauldronLevel -= value;

        if (currentCauldronLevel < 0)
            currentCauldronLevel = 0;
    }
}

public enum HeatStatus
{
    Faded,
    Low,
    Medium,
    High,
    Overheated
}
