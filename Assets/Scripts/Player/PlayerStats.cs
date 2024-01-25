using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //grab current values from save file !!

    public float currentHP = 100f;
    public float maxHP = 100f;
    public float currentStam = 100f;
    public float maxStam = 100f;

    public float damageTakenFromTraps = -20f;

    UIManager ui;

    private void Start()
    {
        ui = UIManager.instance;
        LoadData();
        ui.InitBars(currentHP, maxHP, currentStam, maxStam);
    }

    void LoadData()
    {
        currentHP = DataManager.instance.GetSaveData().currentHP;
    }

    void SaveData()
    {
        DataManager.instance.GetSaveData().currentHP = currentHP;
    }

    public void UpdateCurrentHPValue(float value)
    {
        currentHP += value;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        SaveData();
        ui.UpdateHealthBarValue(currentHP);
    }

    public void UpdateCurrentStamValue(float value)
    {
        currentStam += value;
        if (currentStam < 0)
        {
            currentStam = 0;
        }
        else if (currentStam > maxStam)
        {
            currentStam = maxStam;
        }

        ui.UpdateStaminaBarValue(currentStam);
    }
}
