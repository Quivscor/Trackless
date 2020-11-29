using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSoundController : MonoBehaviour
{
    [SerializeField]
    private float minPitch = 0.7f;

    [SerializeField]
    private float maxPitch = 1.2f;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeSoundPitchBasedOnVelocity(float percentageSpeed)
    {
        if (audioSource)
        {
            audioSource.pitch = (maxPitch - minPitch) * percentageSpeed / 100 + minPitch;
        }
    }
}
