using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAndOutroManager : MonoBehaviour
{
    public List<Cinemachine.CinemachineVirtualCamera> vcams;

    private void Awake()
    {
        FindObjectOfType<TracklessGenerator.MapGenerator>().action += SetVirtualCameras;
    }

    private void SetVirtualCameras()
    {

    }

    public static void PlayIntroSequence()
    {

    }

    public static void PlayOutroSequence()
    {

    }
}
