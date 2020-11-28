using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{ 
    void Start()
    {
        this.transform.Rotate(new Vector3(0, Random.Range(0, 360f), 0));
    }


}
