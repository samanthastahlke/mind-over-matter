using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
OGButton.cs
OGButton (c) Ominous Games 2017
*/

public class OGButton : MonoBehaviour 
{
    private static UIEvents ui;
    public UIEvents.UIButtonType buttonType;

    private void Awake()
    {
        if (ui == null)
            ui = UIEvents.instance;
    }

    public void Press()
    {
        ui.PressButton(buttonType);
    }
}
