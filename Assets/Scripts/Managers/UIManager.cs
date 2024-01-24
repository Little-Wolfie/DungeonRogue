using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Slider healthBar;
    public Slider staminaBar;

    private void Awake()
    {
        instance = this; 
    }

    public void InitBars(float currentHP, float maxHP, float currentStam, float maxStam)
    {
        healthBar.maxValue = currentHP;
        staminaBar.maxValue = maxHP;

        healthBar.value = currentStam;
        staminaBar.value = maxStam;
    }

    public void UpdateHealthBarValue(float value)
    {
        healthBar.value = value;
    }

    public void UpdateHealthBarMaxValue(float value)
    {
        healthBar.maxValue = value;
    }

    public void UpdateStaminaBarValue(float value)
    {
        staminaBar.value = value;
    }

    public void UpdateStaminaBarMaxValue(float value)
    {
        staminaBar.maxValue = value;
    }
}
