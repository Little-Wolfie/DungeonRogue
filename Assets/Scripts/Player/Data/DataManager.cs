using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    // place these values into CreateSaveFile() also!
    // place data to be saved here

    public float currentHP;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField]
    private string _saveFileName = "/TestGame.Data";

    [SerializeField]
    private SaveData _saveData;
    public SaveData GetSaveData()
    {
        return _saveData;
    }

    private SaveData _data;
    BinaryFormatter _formatter;

    public float currentMasterVolume = 0.5f;
    public float currentMusicVolume = 0.5f;
    public float currentSFXVolume = 0.5f;

    static string _MASTERVOLUMEKEY = "MASTERVOLUME";
    static string _MUSICVOLUMEKEY = "MUSICVOLUME";
    static string _SFXVOLUMEKEY = "SFXVOLUME";

    private bool hasLoadedData = false;
    public bool HasLoadedData()
    {
        return hasLoadedData;
    }

    private void Awake()
    {
        instance = this;

        Load();
        LoadSettings();
    }

    private void OnApplicationQuit()
    {
        Save();
        SaveSettings();
    }

    #region Data
    private void CreateSaveFile()
    {
        _data = new SaveData();
        _saveData = new SaveData();

        // can set playerpref here too

        _saveData.currentHP = 100;

        _data = _saveData;

        FileStream file = File.Open(Application.persistentDataPath + _saveFileName, FileMode.Create);
        _formatter.Serialize(file, _data);
        file.Close();

    }

    public void Save()
    {
        _formatter = new BinaryFormatter();

        SaveData data = new SaveData();
        data = _saveData;
        if (File.Exists(Application.persistentDataPath + _saveFileName))
        {
            FileStream file = File.Open(Application.persistentDataPath + _saveFileName, FileMode.Open);
            _formatter.Serialize(file, data);
            file.Close();
        }
        else
        {
            FileStream file = File.Open(Application.persistentDataPath + _saveFileName, FileMode.Create);
            _formatter.Serialize(file, data);
            file.Close();
        }
    }

    public void Load()
    {
        _formatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + _saveFileName))
        {
            FileStream file = File.Open(Application.persistentDataPath + _saveFileName, FileMode.Open);
            _saveData = _formatter.Deserialize(file) as SaveData;
        }
        else
        {
            CreateSaveFile();
        }

        hasLoadedData = true;
    }
    #endregion

    #region Settings
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(_MASTERVOLUMEKEY, currentMasterVolume);

        PlayerPrefs.SetFloat(_MUSICVOLUMEKEY, currentMusicVolume);

        PlayerPrefs.SetFloat(_SFXVOLUMEKEY, currentSFXVolume);
    }

    public void LoadSettings()
    {
        OptionsManager ops = GetComponent<OptionsManager>();

        if (PlayerPrefs.HasKey(_MASTERVOLUMEKEY))
        {
            currentMasterVolume = PlayerPrefs.GetFloat(_MASTERVOLUMEKEY);
            //float mVol = PlayerPrefs.GetFloat(_MASTERVOLUMEKEY);
            //ops.SetMasterVolume(mVol);
            //currentMasterVolume = mVol;
        }
        else
        {
            PlayerPrefs.SetFloat(_MASTERVOLUMEKEY, currentMasterVolume);
        }

        if (PlayerPrefs.HasKey(_MUSICVOLUMEKEY))
        {
            float mVol = PlayerPrefs.GetFloat(_MUSICVOLUMEKEY);
            ops.SetMusicVolume(mVol);
            currentMusicVolume = mVol;
        }
        else
        {
            PlayerPrefs.SetFloat(_MUSICVOLUMEKEY, currentMusicVolume);
        }

        if (PlayerPrefs.HasKey(_SFXVOLUMEKEY))
        {
            float sVol = PlayerPrefs.GetFloat(_SFXVOLUMEKEY);
            ops.SetSFXVolume(sVol);
            currentSFXVolume = sVol;
        }
        else
        {
            PlayerPrefs.SetFloat(_SFXVOLUMEKEY, currentSFXVolume);
        }
    }
    #endregion
}
