using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MainMenuState.cs
MainMenuState (c) Ominous Games 2017
*/

public class MainMenuState : OGState
{
    private static UIEvents ui;

    public MainMenuState(AppManager _app, AppManager.AppState _stateID) : base(_app, _stateID)
    {

    }

    public override void Init()
    {
        base.Init();

        if (ui == null)
            ui = UIEvents.instance;
    }

    public override AppManager.AppState Update()
    {
        if (ui.GetButton(UIEvents.UIButtonType.MM_PLAY))
            return AppManager.AppState.LEVEL_SELECT;
        else if (ui.GetButton(UIEvents.UIButtonType.MM_TUTORIAL))
            return AppManager.AppState.TUTORIAL;
        else if (ui.GetButton(UIEvents.UIButtonType.MM_EXIT))
            return AppManager.AppState.EXIT;

        return stateID;
    }
}
