using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfCoal : IInteractive
{
    public int coalBonus = 10;
    public int coalPickerBonus = 2;

    public AudioClip clip;

    public override void Interact(GameObject interactor)
    {
        if(TrainDataContainer.Instance.wagonsInTrain.Contains(WagonType.CoalPicker))
        {
            foreach(WagonType c in TrainDataContainer.Instance.wagonsInTrain)
            {
                if(c == WagonType.CoalPicker)
                    interactor?.GetComponent<Inventory>()?.AddCoal(coalPickerBonus);
            }
        }
        interactor?.GetComponent<Inventory>()?.AddCoal(coalBonus);
        interactor?.GetComponentInChildren<PickingUp>().PlaySound(clip);
        this.gameObject.SetActive(false);
    }
}
