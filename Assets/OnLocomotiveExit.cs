using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLocomotiveExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().isTrigger = false;
    }
}
