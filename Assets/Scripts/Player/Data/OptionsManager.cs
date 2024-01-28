using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class OptionsManager : MonoBehaviour
{
    public AudioMixerGroup masterMixer, UIMixer, ambientMixer, musicMixer, sfxMixer;

    private void Start()
    {
        SetOptionsUI();
        ApplySettings();
    }

    public void SetMasterVolume()
    {
        if (UIManager.instance.masterVol.value <= 0)
        {
            masterMixer.audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(UIManager.instance.masterVol.value) * 20);
        }
        
        DataManager.instance.currentMasterVolume = UIManager.instance.masterVol.value;
    }
    public void SetMasterVolume(float vol)
    {
        if (vol <= 0)
        {
            masterMixer.audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
        }
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

    public void SetOptionsUI()
    {
        UIManager.instance.masterVol.value = DataManager.instance.currentMasterVolume;
        UIManager.instance.musicVol.value = DataManager.instance.currentMusicVolume;
        UIManager.instance.SFXVol.value = DataManager.instance.currentSFXVolume;
    }
}
