using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerPusher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Deer>())
            other.GetComponent<Deer>().StartRunning(this.transform.root.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Deer>())
            other.GetComponent<Deer>().StopRunning();
    }
}
