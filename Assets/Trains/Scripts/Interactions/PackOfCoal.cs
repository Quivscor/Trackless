using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfCoal : IInteractive
{
    public int coalBonus = 10;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Inventory>()?.AddSteel(coalBonus);
        this.gameObject.SetActive(false);
    }
}
