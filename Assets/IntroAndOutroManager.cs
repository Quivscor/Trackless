using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAndOutroManager : MonoBehaviour
{
    public static List<Cinemachine.CinemachineVirtualCameraBase> vcams;

    private void Awake()
    {
        FindObjectOfType<TracklessGenerator.MapGenerator>().action += SetVirtualCameras;
        //PlayIntroSequence();
    }

    private void SetVirtualCameras()
    {
        vcams.Add(FindObjectOfType<TracklessGenerator.MapGenerator>().spawnObject.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>());
        vcams.Add(FindObjectOfType<TracklessGenerator.MapGenerator>().endObject.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>());
        vcams.Add(FindObjectOfType<TrainController>().GetComponentInChildren<Cinemachine.CinemachineFreeLook>());
    }

    public static void PlayIntroSequence()
    {
        vcams[0].Priority = 15;
    }

    public static void PlayOutroSequence()
    {

    }
}
