using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestShuffler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trees;

    public float treshHold;

    void Start()
    {
        foreach (GameObject tree in trees)
        {
            if(Random.Range(0, 100) > treshHold)
                tree.SetActive(false);
        }
        /*
        int toRemove = Random.Range(0, trees.Count - 1);
        while(toRemove > 0)
        {
            int removedTree = Random.Range(0, trees.Count);
            trees[removedTree].SetActive(false);
            trees.RemoveAt(removedTree);
            toRemove--;
        }*/

    }

}
