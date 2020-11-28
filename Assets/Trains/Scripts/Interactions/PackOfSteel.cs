using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfSteel : IInteractive
{
    public int steelBonus = 10;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Inventory>()?.AddSteel(steelBonus);
        TrainBuilder.Instance.BuildWagon(interactor.GetComponent<TrainManager>(), WagonType.Steel);
        this.gameObject.SetActive(false);
    }
}
