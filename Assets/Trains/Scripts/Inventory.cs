using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int steel = 0;
    private int passengers = 0;
    private int coal = 0;

    public int Steel { get => steel; }
    public int Passengers { get => passengers; }
    public int Coal { get => coal; }

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
}
