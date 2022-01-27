using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clips
{
    Kill,
    PickUp,
    Explosion
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] int audioSourceCounts = 15;
    List<AudioSource> AudioSources;

    [SerializeField]
    List<AudioClip> AudioClips;

    [SerializeField]
    List<Clips> AudioClipEnums;

    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
        #endregion

        InitiateAudioManager();
    }

    void InitiateAudioManager()
    {
        AudioSources = new List<AudioSource>();

        for (int i = 0; i < audioSourceCounts; i++)
        {
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            AudioSources.Add(audioSource);
        }
    }

    public void PlaySFX(Clips AudioClipEnum)
    {
        AudioSource TheSource = UnusedSource();
        if (TheSource == null)
            return;

        PlaySound(TheSource, AudioClipEnum);
    }
    public void PlaySFX(Clips AudioClipEnum, float Volume)
    {
        AudioSource TheSource = UnusedSource();
        if (TheSource == null)
            return;

        PlaySound(TheSource, AudioClipEnum, Volume);
    }

    public void PlaySFX(Clips AudioClipEnum, float Volume, float Pitch)
    {
        AudioSource TheSource = UnusedSource();
        if (TheSource == null)
            return;

        PlaySound(TheSource, AudioClipEnum, Volume, Pitch);
    }

    void PlaySound(AudioSource TheSource, Clips AudioClipEnum)
    {
        if (TheSource == null)
            return;

        TheSource.clip = AudioClips[AudioClipEnums.IndexOf(AudioClipEnum)];

        float Volume = Random.Range(0.5f, 0.75f);

        TheSource.volume = Volume;

        TheSource.Play();
    }
    void PlaySound(AudioSource TheSource, Clips AudioClipEnum, float Volume)
    {
        if (TheSource == null)
            return;

        TheSource.clip = AudioClips[AudioClipEnums.IndexOf(AudioClipEnum)];

        TheSource.volume = Volume;

        TheSource.Play();
    }

    void PlaySound(AudioSource TheSource, Clips AudioClipEnum, float Volume, float Pitch)
    {
        if (TheSource == null)
            return;

        TheSource.clip = AudioClips[AudioClipEnums.IndexOf(AudioClipEnum)];

        TheSource.volume = Volume;
        TheSource.pitch = Pitch;

        TheSource.PlayOneShot(TheSource.clip);
    }

    void StopSource(AudioSource TheSource)
    {
        TheSource.Stop();
    }

    AudioSource UnusedSource()
    {
        foreach (AudioSource source in AudioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
}