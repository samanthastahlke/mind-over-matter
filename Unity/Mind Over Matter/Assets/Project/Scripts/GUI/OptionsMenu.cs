using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
OptionsMenu.cs
OptionsMenu (c) Ominous Games 2017
*/

public class OptionsMenu : MonoBehaviour
{
    public Toggle soundToggle;
    public Toggle tobiiToggle;
    public Toggle neuroskyToggle;
    public Toggle eyeMenuToggle;

    public EyeCaster mainMenuCaster;

    private static AppSettings settings;

    void OnEnable()
    {
        if (settings == null)
            settings = AppSettings.instance;

        soundToggle.isOn = settings.soundOn;
        tobiiToggle.isOn = settings.useTobii;
        neuroskyToggle.isOn = settings.useNeurosky;
        eyeMenuToggle.isOn = settings.eyeMenus;
    }
    
    public void UpdateSound()
    {
        settings.ToggleSound(soundToggle.isOn);
    }

    public void UpdateTobii()
    {
        settings.ToggleTobii(tobiiToggle.isOn);
    }

    public void UpdateNeurosky()
    {
        settings.ToggleNeurosky(neuroskyToggle.isOn);
    }

    public void UpdateEyeMenus()
    {
        settings.ToggleEyeMenus(eyeMenuToggle.isOn, mainMenuCaster);
    }
}
