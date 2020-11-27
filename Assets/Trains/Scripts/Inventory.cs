using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int steel = 0;
    private int passengers = 0;

    public void AddSteel(int value)
    {
        steel += value;
    }

    public void AddPassengers(int value)
    {
        passengers += value;
    }
}
