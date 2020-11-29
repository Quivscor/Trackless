using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankDestroyed : MonoBehaviour
{
    private void Start()
    {
        do
        {
            this.transform.GetChild(0).transform.parent = null;
        } while (this.transform.childCount > 0);
    }
}
