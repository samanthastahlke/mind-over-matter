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

    public float strongBlinkTime = 0.25f;

    public string proxyFocusAxis;
    public string proxyBlinkButton;

    private float blinkTimer = 0.0f;
    private bool blinked = false;
    private bool blinkFrame = false;

    TGCConnectionController neurosky;
    private static AppSettings settings;

    void Awake()
    {
        if (settings == null)
            settings = AppSettings.instance;

        focusInputType = settings.useNeurosky ? FocusInputType.NEUROSKY : FocusInputType.PROXY;
        trackingInputType = settings.useTobii ? TrackingInputType.TOBII : TrackingInputType.PROXY;

        Cursor.visible = !(settings.useTobii && AppManager.instance.settings.eyeMenus);

        neurosky = AppManager.instance.neurosky;
    }

    void Start()
    {
        if (settings.useTobii)
            SetupTobii();

        if (settings.useNeurosky)
            SetupNeurosky();
    }

    public void SetupTobii()
    {
        TobiiAPI.SubscribeGazePointData();
        trackingInputType = TrackingInputType.TOBII;
    }

    public void SetupNeurosky()
    {
        neurosky.Connect();
        focusInputType = FocusInputType.NEUROSKY;
    }

    public void DisableTobii()
    {
        trackingInputType = TrackingInputType.PROXY;
    }

    public void DisableNeurosky()
    {
        neurosky.Disconnect();
        focusInputType = FocusInputType.PROXY;
    }

    void Update()
    {
        if (settings.useTobii)
        {
            if (TobiiAPI.GetUserPresence().IsUserPresent())
            {
                blinked = false;
                blinkTimer = 0.0f;
            }
            else
                blinkTimer += Time.deltaTime;

            if(blinkTimer > strongBlinkTime)
            {
                if (!blinked)
                {
                    blinked = true;
                    blinkFrame = true;
                }
                else
                    blinkFrame = false;
            }
        }
    }

    public bool StrongBlinkDown()
    {
        return Input.GetKeyDown(proxyBlinkButton) || blinkFrame;
    }

    public bool EyesClosed()
    {
        return blinked;
    }

    public float GetScaledFocusLevel()
    {
        if(settings.useNeurosky)
            return (float)neurosky.attention / 100.0f;

        return Mathf.Clamp(Input.GetAxis(proxyFocusAxis), 0.0f, 1.0f);
    }

    public Vector3 GetTrackingPosition()
    {
        if(settings.useTobii)
        {
            if (TobiiAPI.GetGazePoint().IsValid)
                return TobiiAPI.GetGazePoint().Screen;
            else
                return Vector3.zero;
        }

        return Input.mousePosition;
    }
}
