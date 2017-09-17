using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
UIEvents.cs
UIEvents (c) Ominous Games 2017
*/

public class UIEvents : OGSingleton<UIEvents>
{
    public enum UIButtonType
    {
        UI_NIL = -10
    };
    
    private Dictionary<UIButtonType, bool> btnStates;
    private UIButtonType lastButton;

    private AppManager.AppState stateChangeQueued;

	void Awake()
	{
        btnStates = new Dictionary<UIButtonType, bool>();

        foreach(UIButtonType btn in UIButtonType.GetValues(typeof(UIButtonType)))
        {
            btnStates.Add(btn, false);
        }
	}

    protected override void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        base.OnLevelLoad(scene, mode);
        stateChangeQueued = AppManager.AppState.NO_STATE;
    }

    public AppManager.AppState GetStateChangeRequest()
    {
        return stateChangeQueued;
    }

    public void SendStateChangeRequest(AppManager.AppState req)
    {
        stateChangeQueued = req;
    }
	
    public void ResetLastButton()
    {
        lastButton = UIButtonType.UI_NIL;
    }

    public UIButtonType GetLastButton(bool resetState = true)
    {
        UIButtonType result = lastButton;

        if (resetState)
            lastButton = UIButtonType.UI_NIL;

        return result;
    }

    public void PressButton(UIButtonType btn)
    {
        btnStates[btn] = true;
        lastButton = btn;
    }

    public bool GetButton(UIButtonType btn, bool resetState = true)
    {
        bool result = btnStates[btn];

        if (resetState)
            btnStates[btn] = false;

        return result;
    }
}
