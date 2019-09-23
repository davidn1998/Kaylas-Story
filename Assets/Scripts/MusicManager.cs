using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    [SerializeField] float lowPitchRange = 0.8f;
    [SerializeField] float highPitchRange = 1.2f;


    //SINGLETON --------------------------------------------------------

    private static MusicManager instance;

    public static MusicManager Instance { get { return instance; } }

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (instance != null & instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void RandomiseSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }

    public float GetLowPitchRange()
    {
        return lowPitchRange;
    }

    public float GetHighPitchRange()
    {
        return highPitchRange;
    }

    public AudioSource GetMusicSource()
    {
        return musicSource;
    }

    public AudioSource GetSFXSource()
    {
        return  sfxSource;
    }
}
