using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfSteel : IInteractive
{
    public AudioClip clip;

    public int steelBonus = 10;
    public int steelPickerBonus = 2;

    public override void Interact(GameObject interactor)
    {   
        if(TrainDataContainer.Instance.wagonsInTrain.Contains(WagonType.SteelPicker))
        {
            foreach(WagonType c in TrainDataContainer.Instance.wagonsInTrain)
            {
                if(c == WagonType.SteelPicker)
                {
                    interactor?.GetComponent<Inventory>()?.AddSteel(steelPickerBonus);
                }
            }
        }
        interactor?.GetComponent<Inventory>()?.AddSteel(steelBonus);
        TrainBuilder.Instance.BuildWagon(interactor.GetComponent<TrainManager>(), WagonType.Steel);
        interactor?.GetComponentInChildren<PickingUp>().PlaySound(clip);
        this.gameObject.SetActive(false);
    }
}
