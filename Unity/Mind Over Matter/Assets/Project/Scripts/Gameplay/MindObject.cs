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

    private Rigidbody rb;
    private static OGInput input;
    private static GameplayState state;

    private bool hasFocus = false;
    private bool fixedPos = false;
    private Vector3 objVel;
    private Vector3 fixedPosTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (input == null)
            input = OGInput.instance;
    }

    void Start()
    {
        if (state == null)
            state = (GameplayState)AppManager.instance.state;
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
            if (input.GetScaledFocusLevel() >= minimumFocusLevel
                && OGPhysics.ObjectMouseover(input.GetTrackingPosition(), this.gameObject))
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
        if (!fixedPos && (hasFocus || state.focusObject == null))
            CheckFocusChange();
    }
}
