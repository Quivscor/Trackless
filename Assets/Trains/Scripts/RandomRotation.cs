using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{ 
    void Start()
    {
        
        this.transform.Rotate(new Vector3(0, Random.Range(0, 360f), 0));
        float value = Random.Range(-0.3f, 0.6f);
        Vector3 scale = new Vector3(value, value, value);
        this.transform.localScale += scale;
    }


}
