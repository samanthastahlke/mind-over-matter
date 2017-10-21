using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
FocusZone.cs
FocusZone (c) Ominous Games 2017
*/

public class FocusZone : MonoBehaviour
{
    public float minFocus = 0.0f;
    public float maxFocus = 1.0f;

    private static OGInput input;

    void Awake()
    {
        if (input == null)
            input = OGInput.instance;
    }

    void OnTriggerStay(Collider c)
    {
        MindObject obj = c.gameObject.GetComponent<MindObject>();

        if (obj != null)
        {
            float focus = input.GetScaledFocusLevel();

            if (focus < minFocus || focus > maxFocus)
                obj.LoseFocus();
        }
    }

}
