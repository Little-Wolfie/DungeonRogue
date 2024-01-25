using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer musicMixer, sfxMixer;

    public void SetMusicVolume(float vol)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(vol) * 20);
        DataManager.instance.currentMusicVolume = vol;
    }

    public void SetSFXVolume(float vol)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(vol) * 20);
        DataManager.instance.currentSFXVolume = vol;
    }

    public void ApplySettings()
    {
        SetMusicVolume(DataManager.instance.currentMusicVolume);
        SetSFXVolume(DataManager.instance.currentSFXVolume);
    }

    public void SetOptionsUI(Slider mVol, Slider sVol)
    {
        mVol.value = DataManager.instance.currentMusicVolume;
        sVol.value = DataManager.instance.currentSFXVolume;
    }
}
