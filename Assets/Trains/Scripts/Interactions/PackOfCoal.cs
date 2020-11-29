using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOfCoal : IInteractive
{
    public int coalBonus = 10;

    public AudioClip clip;

    public override void Interact(GameObject interactor)
    {
        interactor?.GetComponent<Inventory>()?.AddCoal(coalBonus);
        interactor?.GetComponentInChildren<PickingUp>().PlaySound(clip);
        this.gameObject.SetActive(false);
    }
}
