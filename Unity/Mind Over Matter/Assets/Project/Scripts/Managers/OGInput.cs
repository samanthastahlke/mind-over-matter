using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        focusInputType = FocusInputType.PROXY;
        trackingInputType = TrackingInputType.PROXY;
    }

    public bool StrongBlinkDown()
    {
        return Input.GetKeyDown(proxyBlinkButton);
    }

    public float GetScaledFocusLevel()
    {
        //Proxy placeholder until we have equipment to test.
        return Mathf.Clamp(Input.GetAxis(proxyFocusAxis), 0.0f, 1.0f);
    }

    public Vector3 GetTrackingPosition()
    {
        //Proxy placeholder until we have equipment to test.
        return Input.mousePosition;
    }
}
