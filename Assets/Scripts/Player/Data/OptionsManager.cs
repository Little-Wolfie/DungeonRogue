using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class OptionsManager : MonoBehaviour
{
    public AudioMixerGroup masterMixer, UIMixer, ambientMixer, musicMixer, sfxMixer;

    public void SetMasterVolume(float vol)
    {
        masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
        DataManager.instance.currentMasterVolume = vol;
    }

    public void SetMusicVolume(float vol)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(vol) * 20);
        DataManager.instance.currentMusicVolume = vol;
    }

    public void SetSFXVolume(float vol)
    {
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(vol) * 20);
        DataManager.instance.currentSFXVolume = vol;
    }

    public void ApplySettings()
    {
        SetMasterVolume(DataManager.instance.currentMasterVolume);
        SetMusicVolume(DataManager.instance.currentMusicVolume);
        SetSFXVolume(DataManager.instance.currentSFXVolume);
    }

    public void SetOptionsUI(Slider master, Slider mVol, Slider sVol)
    {
        master.value = DataManager.instance.currentMasterVolume;
        mVol.value = DataManager.instance.currentMusicVolume;
        sVol.value = DataManager.instance.currentSFXVolume;
    }
}
