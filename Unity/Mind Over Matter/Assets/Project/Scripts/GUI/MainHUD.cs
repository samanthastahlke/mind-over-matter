using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
MainHUD.cs
MainHUD (c) Ominous Games 2017
*/

public class MainHUD : OGSingleton<MainHUD>
{
    public bool debugMode = false;

    [Header("NeuroSky")]
    public Sprite connected;
    public Sprite disconnected;
    public Sprite c1;
    public Sprite c2;
    public Sprite c3;

    [Header("Debugging")]
    public GameObject debugGroup;
    public Text debugFocus;
    public GameObject winrar;
    public RectTransform cursor;
    public Image connectSprite;

    private static OGInput input;
    private static TGCConnectionController neurosky;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (input == null)
            input = OGInput.instance;

        if (neurosky == null)
            neurosky = AppManager.instance.neurosky;

        neurosky.UpdatePoorSignalEvent += UpdateSignalStrength;
        debugGroup.SetActive(debugMode);
    }

    void Update()
    {
        if(debugMode)
        {
            debugFocus.text = input.GetScaledFocusLevel().ToString("0.00");
            cursor.anchoredPosition = input.GetTrackingPosition();
        }
    }

    public void UpdateSignalStrength(int signal)
    {
        if (signal < 25)
            connectSprite.sprite = connected;
        else if (signal >= 25 && signal < 51)
            connectSprite.sprite = c3;
        else if (signal >= 51 && signal < 78)
            connectSprite.sprite = c2;
        else if (signal >= 78 && signal < 107)
            connectSprite.sprite = c1;
        else if (signal >= 107)
            connectSprite.sprite = disconnected;      
    }

    public void TriggerWin()
    {
        if(debugMode)
        {
            winrar.SetActive(true);
        }
    }
}
