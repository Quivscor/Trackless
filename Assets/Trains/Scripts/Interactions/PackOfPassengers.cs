using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfPassengers : IInteractive
{
    public int passengers = 1;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Inventory>()?.AddPassengers(passengers);
        TrainBuilder.Instance.BuildWagon(interactor.GetComponent<TrainManager>(), WagonType.LostPassenger);
        this.gameObject.SetActive(false);
    }
}
