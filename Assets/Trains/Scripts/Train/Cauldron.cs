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

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        currentCauldronLevel = cauldronMaxLevel;
    }

    private void Update()
    {
        if (InputManager.Data.cauldronBoost)
            AddCoalToCauldron();

        currentCauldronLevel -= burningSpeed * Time.deltaTime;

        if (currentCauldronLevel < 0)
            currentCauldronLevel = 0;
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
}
