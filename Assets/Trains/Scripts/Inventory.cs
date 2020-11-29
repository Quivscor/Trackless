using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int steel = 0;
    private int passengers = 0;
    private int coal = 0;

    private static int HitTrees = 0;

    public int Steel { get => steel; }
    public int Passengers { get => passengers; }
    public int Coal { get => coal; }

    private void Start()
    {
        AddCoal(5);
    }

    public void AddSteel(int value)
    {
        steel += value;
    }

    public void AddPassengers(int value)
    {
        passengers += value;
    }

    public void AddCoal(int value)
    {
        coal += value;
    }

    public void SpendCoal(int value)
    {
        coal -= value;

        if (coal < 0)
            coal = 0;
    }

    public static void TreeHit()
    {
        Inventory.HitTrees++;
    }

    public static int GetHitTrees() { return HitTrees; }

    [Header("Points")]
    [SerializeField]
    private static float basePoints = 0.0f;

    [SerializeField]
    private static float passengerModifier = 50000.0f;

    public int GetPoints()
    {
        Debug.Log((basePoints + (1.0f / TimeCounter.CTime()) * passengerModifier * passengers));
        return (int)(basePoints + (1.0f / TimeCounter.CTime()) * passengerModifier * passengers);
    }
}
