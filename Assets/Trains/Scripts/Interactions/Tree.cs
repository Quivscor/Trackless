using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : IInteractive
{
    [SerializeField]
    private float heatLostOnHit = 2.5f;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Cauldron>()?.SubtractHeatLevel(heatLostOnHit);

        Inventory.TreeHit();
    }
}
