using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cauldron : MonoBehaviour
{
    public Inventory inventory = null;

    [SerializeField]
    private float burningSpeed = 1.0f;

    [SerializeField]
    private float burningBoostPerCoal = 20.0f;
    [SerializeField] private float burningBoostTime = 1f;
    private float boostRemainingToAdd;

    public float timeToFreeze = 10f;
    private float currentTimeToFreeze;
    public static bool frozeToDeath = false;
    public TextMeshProUGUI freezeWarning;

    [SerializeField]
    private float cauldronMaxLevel = 150.0f;

    private float currentCauldronLevel = 20.0f;

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
        freezeWarning = FindObjectOfType<WarningTextDummy>().GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        currentCauldronLevel = 20f;
        SetHeatStatus();
    }

    private void Update()
    {
        if (InputManager.Data.cauldronBoost)
            AddCoalToCauldron();

        if (InputManager.Data.isBrake)
            currentCauldronLevel -= burningSpeed * 25 * Time.deltaTime;

        if(boostRemainingToAdd > 0)
        {
            float boost = burningBoostPerCoal * Time.deltaTime;
            if (boost > boostRemainingToAdd)
                boost = boostRemainingToAdd;
            currentCauldronLevel += boost;
            if (currentCauldronLevel > CauldronMaxLevel)
            {
                currentCauldronLevel = CauldronMaxLevel;
            }
            boostRemainingToAdd -= boost;
        }
        else
            currentCauldronLevel -= burningSpeed * Time.deltaTime;

        if (currentCauldronLevel <= 0)
        {
            currentCauldronLevel = 0;
            UpdateFreezing();
        }
        else
            ResetFreezing();

        SetHeatStatus();
    }

    private void UpdateFreezing()
    {
        currentTimeToFreeze += Time.deltaTime;
        if(!frozeToDeath)
            freezeWarning.gameObject.SetActive(true);
        freezeWarning.text = "Train will freeze in " + ((int)timeToFreeze - (int)currentTimeToFreeze).ToString() + " seconds!";
        if (currentTimeToFreeze >= timeToFreeze && !frozeToDeath)
        {
            FindObjectOfType<EndGameController>().GameOverLostHeat();
            frozeToDeath = true;
        }
    }

    private void ResetFreezing()
    {
        currentTimeToFreeze = 0;
        freezeWarning.gameObject.SetActive(false);
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
        //currentCauldronLevel += burningBoostPerCoal;

        //if (currentCauldronLevel > cauldronMaxLevel)
        //    currentCauldronLevel = cauldronMaxLevel;
        boostRemainingToAdd += burningBoostPerCoal;
    }

    private void AddCoalToCauldron()
    {
        if (inventory)
        {
            if (inventory.Coal > 0 && (currentCauldronLevel + boostRemainingToAdd < CauldronMaxLevel))
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
