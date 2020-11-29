using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "WagonShop/WagonData")]
public class WagonShop : ScriptableObject
{
    public Sprite image;
    public WagonType wagonType;
    public string description;
    public int steelPrice;
}
