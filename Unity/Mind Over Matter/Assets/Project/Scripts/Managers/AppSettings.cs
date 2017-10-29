using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
AppSettings.cs
AppSettings (c) Ominous Games 2017
*/

public class AppSettings : OGSingleton<AppSettings>
{
    public bool eyeMenus = false;
    public bool useTobii = false;
    public bool useNeurosky = false;
    public bool soundOn = true;

    void Awake()
    {
        eyeMenus = PlayerPrefs.GetInt("eyeMenus", 0) != 0;
        useTobii = PlayerPrefs.GetInt("useTobii", 0) != 0;
        useNeurosky = PlayerPrefs.GetInt("useNeurosky", 0) != 0;
        soundOn = PlayerPrefs.GetInt("soundOn", 1) != 0;

        ToggleSound(soundOn);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("eyeMenus", (eyeMenus) ? 1 : 0);
        PlayerPrefs.SetInt("useTobii", (useTobii) ? 1 : 0);
        PlayerPrefs.SetInt("useNeurosky", (useNeurosky) ? 1 : 0);
        PlayerPrefs.SetInt("soundOn", (soundOn) ? 1 : 0);
    }
    
    public void ToggleTobii(bool _useTobii)
    {
        useTobii = _useTobii;

        if (useTobii)
            OGInput.instance.SetupTobii();
        else
            OGInput.instance.DisableTobii();
    }

    public void ToggleNeurosky(bool _useNeurosky)
    {
        useNeurosky = _useNeurosky;

        if (useNeurosky)
            OGInput.instance.SetupNeurosky();
        else
            OGInput.instance.DisableNeurosky();
    }

    public void ToggleSound(bool _soundOn)
    {
        soundOn = _soundOn;
        AudioListener.volume = (soundOn) ? 0.0f : 1.0f;
    }

    public void ToggleEyeMenus(bool _eyeMenus, EyeCaster eyeCaster)
    {
        eyeMenus = _eyeMenus;
        
        eyeCaster.enabled = false;
        eyeCaster.enabled = true;
        Cursor.visible = !eyeMenus;

        EventSystem.current.SetSelectedGameObject(null);
    }
}
