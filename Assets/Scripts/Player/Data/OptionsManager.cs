using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class OptionsManager : MonoBehaviour
{
    public AudioMixerGroup masterMixer, UIMixer, ambientMixer, musicMixer, SFXMixer;

    private void Start()
    {
        SetOptionsUI();
        ApplySettings();
    }

    #region Volumes
    #region Master Volume
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
    #endregion

    #region Music Volume
    public void SetMusicVolume()
    {
        if (UIManager.instance.musicVol.value <= 0)
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(UIManager.instance.musicVol.value) * 20);
        }

        DataManager.instance.currentMusicVolume = UIManager.instance.musicVol.value;
    }
    public void SetMusicVolume(float vol)
    {
        if (vol <= 0)
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(vol) * 20);
        }
    }
    #endregion

    #region SFX Volume
    public void SetSFXVolume()
    {
        if (UIManager.instance.SFXVol.value <= 0)
        {
            SFXMixer.audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            SFXMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(UIManager.instance.SFXVol.value) * 20);
        }

        DataManager.instance.currentSFXVolume = UIManager.instance.SFXVol.value;
    }
    public void SetSFXVolume(float vol)
    {
        if (vol <= 0)
        {
            SFXMixer.audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            SFXMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(vol) * 20);
        }
    }
    #endregion

    #region UI Volume
    public void SetUIVolume()
    {
        if (UIManager.instance.UIVol.value <= 0)
        {
            UIMixer.audioMixer.SetFloat("UIVolume", -80);
        }
        else
        {
            UIMixer.audioMixer.SetFloat("UIVolume", Mathf.Log10(UIManager.instance.UIVol.value) * 20);
        }

        DataManager.instance.currentUIVolume = UIManager.instance.UIVol.value;
    }
    public void SetUIVolume(float vol)
    {
        if (vol <= 0)
        {
            UIMixer.audioMixer.SetFloat("UIVolume", -80);
        }
        else
        {
            UIMixer.audioMixer.SetFloat("UIVolume", Mathf.Log10(vol) * 20);
        }
    }
    #endregion

    #region Ambient Volume
    public void SetAmbientVolume()
    {
        if (UIManager.instance.ambientVol.value <= 0)
        {
            ambientMixer.audioMixer.SetFloat("AmbientVolume", -80);
        }
        else
        {
            ambientMixer.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(UIManager.instance.ambientVol.value) * 20);
        }

        DataManager.instance.currentAmbientVolume = UIManager.instance.ambientVol.value;
    }
    public void SetAmbientVolume(float vol)
    {
        if (vol <= 0)
        {
            ambientMixer.audioMixer.SetFloat("AmbientVolume", -80);
        }
        else
        {
            ambientMixer.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(vol) * 20);
        }
    }
    #endregion
    #endregion

    public void ApplySettings()
    {
        SetMasterVolume(DataManager.instance.currentMasterVolume);
        SetMusicVolume(DataManager.instance.currentMusicVolume);
        SetSFXVolume(DataManager.instance.currentSFXVolume);
        SetAmbientVolume(DataManager.instance.currentAmbientVolume);
        SetUIVolume(DataManager.instance.currentUIVolume);
    }

    public void SetOptionsUI()
    {
        UIManager.instance.masterVol.value = DataManager.instance.currentMasterVolume;
        UIManager.instance.musicVol.value = DataManager.instance.currentMusicVolume;
        UIManager.instance.SFXVol.value = DataManager.instance.currentSFXVolume;
        UIManager.instance.UIVol.value = DataManager.instance.currentUIVolume;
        UIManager.instance.ambientVol.value = DataManager.instance.currentAmbientVolume;
    }
}
