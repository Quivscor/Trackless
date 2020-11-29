using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndArrowConditionShowUp : MonoBehaviour
{
    private GameObject arrow;
    private GameObject blockade;

    private void Awake()
    {
        arrow = transform.GetChild(0).gameObject;
        blockade = transform.GetChild(1).gameObject;
    }

    public void ActivateArrow()
    {
        arrow.SetActive(true);
        blockade.SetActive(false);

        foreach (Plank p in FindObjectsOfType<Plank>())
        {
            p.canBreak = true;
        }
    }
}
