using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
GameplayState.cs
GameplayState (c) Ominous Games 2017
*/

public class GameplayState : OGState
{
    public GameObject focusObject { get; protected set; }
    private bool winState = false;
    
    public GameplayState(AppManager _app, AppManager.AppState _stateID) : base(_app, _stateID)
    {

    }

    public override AppManager.AppState Update()
    {
        OGPhysics.CheckMindObjects(app.input.GetTrackingPosition());

        if (Input.GetKeyDown(KeyCode.Escape))
            return AppManager.AppState.LEVEL_SELECT;

        if(winState && app.input.StrongBlinkDown())
        {
            if (stateID == AppManager.AppState.L12)
                return AppManager.AppState.LEVEL_SELECT;
            else
                return stateID + 1;
        }

        return stateID;
    }

    public void SetFocusObject(GameObject focusObject)
    {
        this.focusObject = focusObject;
    }

    public void TriggerWinState()
    {
        winState = true;
    }
}
