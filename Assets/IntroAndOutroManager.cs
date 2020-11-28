using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAndOutroManager : MonoBehaviour
{
    public List<Cinemachine.CinemachineVirtualCameraBase> vcams = new List<Cinemachine.CinemachineVirtualCameraBase>();

    private void Awake()
    {
        FindObjectOfType<TracklessGenerator.MapGenerator>().action += SetVirtualCameras;
    }

    private void SetVirtualCameras()
    {
        GameObject ob = FindObjectOfType<TracklessGenerator.MapGenerator>().spawnObject;
        vcams.Add(ob.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>());
        ob = FindObjectOfType<TracklessGenerator.MapGenerator>().endObject;
        vcams.Add(ob.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>());
        vcams.Add(FindObjectOfType<TrainController>().GetComponentInChildren<Cinemachine.CinemachineFreeLook>());

        PlayIntroSequence();
    }

    public void PlayIntroSequence()
    {
        vcams[0].Priority = 15;
        StartCoroutine(ChangeVCam0AfterTime(0, 1.5f));
    }

    public void PlayOutroSequence()
    {
        vcams[1].Priority = 15;
    }

    private IEnumerator ChangeVCam0AfterTime(int value, float time)
    {
        yield return new WaitForSeconds(time);
        vcams[0].Priority = value;
    }

    private IEnumerator ChangeVCam1AfterTime(int value, float time)
    {
        yield return new WaitForSeconds(time);
        vcams[1].Priority = value;
    }
}
