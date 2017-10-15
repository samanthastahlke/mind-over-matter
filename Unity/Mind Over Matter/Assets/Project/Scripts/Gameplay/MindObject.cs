using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MindObject.cs
MindObject (c) Ominous Games 2017
*/

[RequireComponent(typeof(Rigidbody))]
public class MindObject : MonoBehaviour
{
    public float minimumFocusLevel = 0.25f;
    public float maxMoveSpeed = 4.0f;
    public float dampingTime = 0.25f;
    public float colSpeed = 2.0f;

    private Rigidbody rb;
    private static OGInput input;
    private static AppManager app;
    private static GameplayState state;

    private bool hasFocus = false;
    private bool hasEyes = false;
    private bool fixedPos = false;
    private Vector3 objVel;
    private Vector3 fixedPosTarget;

    private Material mat;
    private float glowAlpha = 0.0f;
    private Color col = Color.white;
    private Vector3 colVel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (input == null)
            input = OGInput.instance;

        if (app == null)
            app = AppManager.instance;

        mat = GetComponent<Renderer>().material;
    }

    void Start()
    {
        if (state == null)
            state = (GameplayState)app.state;
    }

    void GainFocus()
    {
        hasFocus = true;
        state.SetFocusObject(this.gameObject);
        rb.useGravity = false;
        objVel = Vector3.zero;
    }

    void LoseFocus()
    {
        hasFocus = false;
        state.SetFocusObject(null);
        rb.useGravity = true;
        objVel = Vector3.zero;
    }

    void CheckFocusChange()
    {
        if(hasFocus)
        {
            if (input.StrongBlinkDown())
                LoseFocus();
        }
        else
        {
            if (input.GetScaledFocusLevel() >= minimumFocusLevel && hasEyes)
                GainFocus();
        }
    }

    public void FixPosition(Vector3 fixedPosTarget)
    {
        hasFocus = false;
        fixedPos = true;
        state.SetFocusObject(null);
        rb.useGravity = false;

        this.fixedPosTarget = fixedPosTarget;
    }

    void FixedUpdate()
    {
        if(hasFocus)
        {
            Vector3 trackPos = input.GetTrackingPosition();
            trackPos.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);

            Vector3 targetPos = Camera.main.ScreenToWorldPoint(trackPos);
            targetPos.z = transform.position.z;

            Vector3 dampingPos = Vector3.SmoothDamp(transform.position, targetPos, ref objVel, dampingTime, maxMoveSpeed);

            rb.MovePosition(dampingPos);
        }
        else if(fixedPos)
        {
            Vector3 dampingPos = Vector3.SmoothDamp(transform.position, fixedPosTarget, ref objVel, dampingTime, maxMoveSpeed);
            rb.MovePosition(dampingPos);
        }
    }
    
    void Update() 
    {
        hasEyes = OGPhysics.ObjectMouseover(input.GetTrackingPosition(), this.gameObject);

        if (!fixedPos && (hasFocus || state.focusObject == null))
            CheckFocusChange();

        float targGlowAlpha = (hasEyes || hasFocus) ? 1.0f : 0.0f;
        Color targColor = (hasFocus) ? app.focusColor.Evaluate(input.GetScaledFocusLevel()) : Color.white;

        Vector3 source = new Vector3(col.r, col.g, col.b);
        Vector3 dest = new Vector3(targColor.r, targColor.g, targColor.b);

        Vector3 colResult = Vector3.SmoothDamp(source, dest, ref colVel, dampingTime, colSpeed);

        col = new Color(colResult.x, colResult.y, colResult.z);

        float alphaChange = targGlowAlpha - glowAlpha;

        if (Mathf.Abs(alphaChange) > Time.deltaTime * colSpeed)
            alphaChange = (alphaChange < 0) ? -Time.deltaTime * colSpeed : Time.deltaTime * colSpeed;

        glowAlpha += alphaChange;

        mat.SetColor("_EmissionColor", new Color(glowAlpha, glowAlpha, glowAlpha));
        mat.SetColor("_Color", col);
        
    }
}
