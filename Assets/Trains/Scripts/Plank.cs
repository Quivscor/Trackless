﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    public GameObject destroyedPlank;
    public bool canBreak = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Locomotive") && other.gameObject.transform.GetComponent<TrainManager>() && canBreak)
        {
            GameObject destroyedPlankGO = Instantiate(destroyedPlank, this.transform.position, this.transform.rotation);
            Destroy(destroyedPlankGO, 4.0f);
            this.gameObject.SetActive(false);
        }
    }
}
