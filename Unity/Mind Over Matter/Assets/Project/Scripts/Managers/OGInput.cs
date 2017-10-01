using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

/*
OGInput.cs
OGInput (c) Ominous Games 2017
*/

public class OGInput : OGSingleton<OGInput>
{
    public enum FocusInputType
    {
        PROXY,
        NEUROSKY
    };

    public enum TrackingInputType
    {
        PROXY,
        TOBII
    };

    public FocusInputType focusInputType { get; protected set; }
    public TrackingInputType trackingInputType { get; protected set; }

    public string proxyFocusAxis;
    public string proxyBlinkButton;

    public bool useNeurosky = false;
    public bool useTobii = false;

    TGCConnectionController neurosky;

    void Awake()
    {
        focusInputType = FocusInputType.PROXY;
        trackingInputType = TrackingInputType.PROXY;

        neurosky = AppManager.instance.neurosky;
    }

    public bool StrongBlinkDown()
    {
        return Input.GetKeyDown(proxyBlinkButton);
    }

    public float GetScaledFocusLevel()
    {
        if(useNeurosky)
            return (float)neurosky.attention / 100.0f;

        return Mathf.Clamp(Input.GetAxis(proxyFocusAxis), 0.0f, 1.0f);
    }

    public Vector3 GetTrackingPosition()
    {
        if(useTobii)
            return TobiiAPI.GetGazePoint().Screen;

        return Input.mousePosition;
    }
}
