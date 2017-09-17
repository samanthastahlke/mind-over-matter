using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
OGState.cs
OGState (c) Ominous Games 2017
*/
public class OGState
{
    protected AppManager app;
    public AppManager.AppState stateID { get; protected set; }

    public OGState(AppManager _app, AppManager.AppState _stateID)
    {
        app = _app;
        stateID = _stateID;
    }

	public virtual void Init()
    {

    }

    public virtual void Cleanup()
    {

    }

    public virtual bool IsPaused()
    {
        return false;
    }

    public virtual bool HasInputFocus()
    {
        return true;
    }

	public virtual AppManager.AppState Update() 
	{
        return stateID;
	}
}
