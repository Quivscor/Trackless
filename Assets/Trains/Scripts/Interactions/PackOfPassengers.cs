using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfPassengers : IInteractive
{
    public AudioClip clip;

    public int passengers = 1;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Inventory>()?.AddPassengers(passengers);
        TrainBuilder.Instance.BuildWagon(interactor.GetComponent<TrainManager>(), WagonType.LostPassenger);
        interactor?.GetComponentInChildren<PickingUp>().PlaySound(clip);
        this.gameObject.SetActive(false);
    }
}
