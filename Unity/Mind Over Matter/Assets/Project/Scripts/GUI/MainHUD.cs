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

    [Header("Debugging")]
    public Text debugFocus;
    public GameObject winrar;

    private static OGInput input;

    void Awake()
    {
        if (input == null)
            input = OGInput.instance;
    }

    void Update()
    {
        if(debugMode)
        {
            debugFocus.text = input.GetScaledFocusLevel().ToString("0.00");
        }
    }

    public void TriggerWin()
    {
        if(debugMode)
        {
            winrar.SetActive(true);
        }
    }
}
