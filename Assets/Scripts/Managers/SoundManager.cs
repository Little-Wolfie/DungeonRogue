using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum SoundType
    {
        Music,
        SFX,
        UI
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public SoundType type;
        public AudioClip clip;
        [Range(0, 1f)]
        public float volume = 0.5f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public string currentPlayingMusic;

    public AudioMixerGroup musicMixerGroup, sfxMixerGroup;

    public Sound[] sounds;

    private void Awake()
    {
        instance = this;

        Init();
    }

    private void Init()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume; s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.type == SoundType.Music)
            {
                if (musicMixerGroup == null)
                {
                    Debug.LogError("Music Mixer group not found");
                    continue;
                }
                s.source.outputAudioMixerGroup = musicMixerGroup;
            }
            else if (s.type == SoundType.SFX)
            {
                if (sfxMixerGroup == null)
                {
                    Debug.LogError("SFX Mixer group not found");
                    continue;
                }
                s.source.outputAudioMixerGroup = sfxMixerGroup;
            }
        }
    }

    public void MusicTransition(string name)
    {
        if (currentPlayingMusic != "")
        {
            Stop(currentPlayingMusic);
        }

        Play(name);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Play() - Sound not found: " + name);
            return;
        }
        s.source.volume = 0;
        s.source.Play();
        s.source.volume = s.volume;

        if (s.type == SoundType.Music)
        {
            currentPlayingMusic = s.name;
        }
    }

    public void Play(string name, float pitch, float vol)
    {
        //leave pitch and vol zero to just play a sound normally

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (pitch > 0)
        {
            s.source.pitch += pitch;
        }
        if (vol > 0)
        {
            s.source.volume += vol;
        }

        if (s == null)
        {
            Debug.LogError("Play() - Sound not found: " + name);
            return;
        }

        s.source.Play();

        if (s.type == SoundType.Music)
        {
            currentPlayingMusic = s.name;
        }

        if (pitch > 0)
        {
            s.source.pitch = s.pitch;
        }
        if (vol > 0)
        {
            s.source.volume = s.volume;
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Stop() - Sound not found: " + name);
            return;
        }

        StartCoroutine(VolumeFade(s, 0f, 0.2f));
    }

    IEnumerator VolumeFade(Sound s, float _EndVolume, float _FadeLength)
    {
        float _StartVolume = s.source.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength)
        {

            s.source.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0)
        {
            s.source.Stop();
            s.source.volume = s.volume;
        }
    }
}
