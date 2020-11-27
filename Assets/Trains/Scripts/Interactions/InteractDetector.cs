using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IInteractive interactive = other.GetComponent<IInteractive>();

        if (interactive != null)
            interactive.Interact(this.transform.root.gameObject);
    }
}
