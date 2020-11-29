using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndArrowConditionShowUp : MonoBehaviour
{
    private GameObject arrow;

    private void Awake()
    {
        arrow = transform.GetChild(0).gameObject;
    }

    public void ActivateArrow()
    {
        arrow.SetActive(true);
    }
}
