using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlareLauncher : MonoBehaviour
{
    public float launchCooldown;
    private float launchOffset;
    private float launchCooldownCurrent;
    public Vector2 launchOffsetMinMax;

    private AudioSource soundSource;
    private VisualEffect flareEffect;

    private void Awake()
    {
        flareEffect = GetComponent<VisualEffect>();
        soundSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        flareEffect.Stop();

        launchOffset = Random.Range(launchOffsetMinMax.x, launchOffsetMinMax.y);
        launchCooldownCurrent = launchCooldown + launchOffset;
    }

    private void Update()
    {
        launchCooldownCurrent -= Time.deltaTime;
        if(launchCooldownCurrent <= 0)
        {
            launchCooldownCurrent = launchCooldown;
            LaunchFlare();
        }
    }

    public void LaunchFlare()
    {
        flareEffect.Play();
        soundSource.PlayOneShot(soundSource.clip);
        StartCoroutine(StopAfterTime(1.5f));
    }

    private IEnumerator StopAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        flareEffect.Stop();
    }
}
