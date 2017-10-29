using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
LevelSelectState.cs
LevelSelectState (c) Ominous Games 2017
*/

public class LevelSelectState : GameplayState
{
    public AppManager.AppState queuedState;
    public LevelSelectState(AppManager _app, AppManager.AppState _stateID) : base(_app, _stateID)
    {

    }

    public override void Init()
    {
        base.Init();
        queuedState = AppManager.AppState.NO_STATE;
    }

    public override AppManager.AppState Update()
    {
        OGPhysics.CheckMindObjects(app.input.GetTrackingPosition());

        if (Input.GetKeyDown(KeyCode.Escape) || app.ui.GetButton(UIEvents.UIButtonType.PL_QUIT))
            return AppManager.AppState.MAIN_MENU;

        if (queuedState != AppManager.AppState.NO_STATE)
            return queuedState;

        return stateID;
    }

    public void QueueLevel(int level)
    {
        queuedState = AppManager.AppState.TUTORIAL + level;
    }
}
