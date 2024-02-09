using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
 
public class UISettingContontoller : MonoBehaviour
{
    ///////////
    /// Game constants for player preferences
    public const string graphics = "graphics";
    public const string sfx = "sfx";
    public const string music = "music";
    public const string brightness = "brightness";
    public const string vibration = "vibration";
    /////

    [Header("Setting")]
    public GameObject settingPanel;
    public TMP_Dropdown graphicsQualtiy;
    public Slider musicLevel;
    public Slider sfxLevel;
    public Slider brightnessLevel;
    public Toggle vibrationToggle;


    public void SetDefaultSettngsPreferences()
    {

        graphicsQualtiy.value = PlayerPrefs.GetInt(graphics, 1);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(graphics)+1);
        sfxLevel.value = PlayerPrefs.GetFloat(sfx, 1);
        AudioController.audioController.SetSFXAudioSourcesVlolume();
        musicLevel.value = PlayerPrefs.GetFloat(music, 1);
        AudioController.audioController.SetMusiceAudioSourcesVlolume();
        brightnessLevel.value = PlayerPrefs.GetFloat(brightness, 1);
        Screen.brightness = PlayerPrefs.GetFloat(brightness, 1);
        vibrationToggle.isOn = Convert.ToBoolean( PlayerPrefs.GetInt(vibration, 1));
       

    }


    private void Start()
    {
        SetDefaultSettngsPreferences();
    }
    

    public void OnChangeGraphicsQuality(int lvl)
    {
        PlayerPrefs.SetInt(graphics, lvl);
        QualitySettings.SetQualityLevel(lvl+1);
        AudioController.audioController.PlayClickAudio();
    }
    public void OnChangeSFXVolume(float lvl)
    {
        PlayerPrefs.SetFloat(sfx, lvl);
        AudioController.audioController.SetSFXAudioSourcesVlolume();
    }
    public void OnChangeMusicVolume(float lvl)
    {
        PlayerPrefs.SetFloat(music, lvl);
        AudioController.audioController.SetMusiceAudioSourcesVlolume();
   
    }
    public void OnChangeBrightness(float lvl)
    {
        PlayerPrefs.SetFloat(brightness, lvl);
        Screen.brightness = PlayerPrefs.GetFloat(brightness);
    }
    public void OnChangeVibration(bool isVibrating)
    {
        PlayerPrefs.SetInt(vibration, Convert.ToInt32(isVibrating));
        AudioController.audioController.PlayClickAudio();

    }
}
