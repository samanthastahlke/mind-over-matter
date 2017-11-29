using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : OGSingleton<SoundManager>
{
    public AudioSource musicChannel;
    public AudioSource fxChannel;
    public AudioSource ambientChannel;
    public AudioSource collisionChannel;
    public AudioSource spatialChannel;

    public AudioClip levelEndFX;
    public AudioClip levelCheekFX;
    public List<AudioClip> collisionNoises;
    public List<AudioClip> spatialNoises;

    public float minSoundTime = 5.0f;
    public float maxSoundTime = 15.0f;

    private float soundTimer = 0.0f;
    private float nextSoundThreshold = 0.0f;
    public float minSoundDist = 1.0f;
    public float maxSoundDist = 10.0f;

    private void Awake()
    {
        ResetSoundClock();
    }

    private void Update()
    {
        soundTimer += Time.deltaTime;

        if(soundTimer > nextSoundThreshold)
        {
            ResetSoundClock();
            Vector3 dir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

            if (dir.magnitude < 0.01f)
                dir = Vector3.forward;

            Vector3 pos = Camera.main.transform.position + Random.Range(minSoundDist, maxSoundDist) * dir.normalized;
            PlaySpatialFX(pos);
        }
	}

    public void PlayLevelEndFX(bool cheeky = false)
    {
        fxChannel.PlayOneShot((cheeky) ? levelCheekFX : levelEndFX);
    }

    public void PlayCollisionFX()
    {
        if (collisionNoises.Count < 1)
            return;

        AudioClip fxClip = collisionNoises[Random.Range(0, collisionNoises.Count - 1)];
        collisionChannel.PlayOneShot(fxClip);
    }

    public void PlaySpatialFX(Vector3 worldPos)
    {
        if (spatialNoises.Count < 1)
            return;

        AudioClip fxClip = spatialNoises[Random.Range(0, spatialNoises.Count)];

        spatialChannel.gameObject.transform.position = worldPos;
        spatialChannel.PlayOneShot(fxClip);
    }

    private void ResetSoundClock()
    {
        soundTimer = 0.0f;
        nextSoundThreshold = Random.Range(minSoundTime, maxSoundTime);
    }
}
