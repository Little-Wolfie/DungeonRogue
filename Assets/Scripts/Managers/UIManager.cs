using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject optionsPanel;
    public bool isMenuOpen = false;
    public GameObject cameraControlsObject;

    public Slider healthBar;
    public Slider staminaBar;

    public Slider masterVol, musicVol, SFXVol, UIVol, ambientVol;

    private void Awake()
    {
        instance = this; 
    }

    public void ToggleOptionsMenu()
    {
        SoundManager.instance.Play("OpenInventory", 0, 0, SoundManager.instance.ui);

        optionsPanel.SetActive(!optionsPanel.activeInHierarchy);
        isMenuOpen = optionsPanel.activeInHierarchy;
        cameraControlsObject.SetActive(!optionsPanel.activeInHierarchy);

        if (optionsPanel.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        }
    }

    public void InitBars(float currentHP, float maxHP, float currentStam, float maxStam)
    {
        healthBar.maxValue = maxHP;
        staminaBar.maxValue = maxStam;

        healthBar.value = currentHP;
        staminaBar.value = currentStam;
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
