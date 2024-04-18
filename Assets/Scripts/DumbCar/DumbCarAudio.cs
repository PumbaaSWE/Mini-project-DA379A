using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DumbCarAudio : MonoBehaviour
{

    public AudioClip lowRevClip;                                              
    [Range(0, 1)] public float lowVolumeMultiplier = 1;
    public AudioClip highRevClip;                                              
    [Range(0, 1)] public float highVolumeMultiplier = 1;

    public float pitchMultiplier = 1f;                                          
    public float lowPitchMin = 1f;                                              
    public float lowPitchMax = 3f;                                              
    public float highPitchMultiplier = 0.25f;
    public float maxDistance = 999;
    public float dopplerLevel = 1;
    public bool useDoppler = true;


    private DumbCar car;

    private AudioSource highRevSource; 
    private AudioSource lowRevSource; 

    //private bool started;


    private static AudioListener _activeAudioListener;
    private static AudioListener ActiveAudioListener
    {
        get
        {
            if (!_activeAudioListener || !_activeAudioListener.isActiveAndEnabled)
            {
                var audioListeners = FindObjectsOfType<AudioListener>(false);
                _activeAudioListener = Array.Find(audioListeners, audioListener => audioListener.enabled); // No need to check isActiveAndEnabled, FindObjectsOfType already filters out inactive objects.
            }

            return _activeAudioListener;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<DumbCar>();
        StartSound();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //bool inRange = (ActiveAudioListener.transform.position - transform.position).sqrMagnitude < maxDistance * maxDistance;

        //if (started && !inRange)
        //{
        //    StopSound();
        //    return;
        //}

        //if (!started && inRange) {
        //    StartSound();
        //}

        float pitch = Mathf.LerpUnclamped(lowPitchMin, lowPitchMax, (car.EngineRPM-car.MinRPM) / (car.MaxRPM - car.MinRPM)); //was unclamped
        pitch = Mathf.Min(lowPitchMax, pitch);
        

        highRevSource.pitch = pitch * pitchMultiplier * highPitchMultiplier;
        lowRevSource.pitch = pitch * pitchMultiplier;

        float highFade = Mathf.InverseLerp(car.MinRPM,  car.MaxRPM, car.EngineRPM);
        float lowFade = 1 - highFade;

        highRevSource.volume = (1 - lowFade * lowFade) * highVolumeMultiplier;
        lowRevSource.volume = (1 - highFade * highFade) * lowVolumeMultiplier;
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.spatialBlend = 1;
        source.loop = true;
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxDistance;
        source.dopplerLevel = useDoppler ? dopplerLevel : 0;
        return source;
    }

    private void StartSound()
    {
        highRevSource = SetUpEngineAudioSource(highRevClip);
        lowRevSource = SetUpEngineAudioSource(lowRevClip);
        //started = true;
    }


    //private void StopSound()
    //{
    //    Destroy(highRevSource);
    //    Destroy(lowRevSource);
    //    started = false;
    //}

    private void OnValidate()
    {
        if(highRevSource) highRevSource.dopplerLevel = useDoppler ? dopplerLevel : 0;
        if(lowRevSource) lowRevSource.dopplerLevel = useDoppler ? dopplerLevel : 0;
    }
}
