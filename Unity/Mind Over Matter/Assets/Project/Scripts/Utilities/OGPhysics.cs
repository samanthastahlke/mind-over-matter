using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
OGPhysics.cs
OGPhysics (c) Ominous Games 2017
*/

public class OGPhysics
{
    public const float RAY_THRES = 40.0f;

    public static bool ObjectMouseover(Vector3 screenPos, GameObject obj)
    {
        RaycastHit hit;
        Ray testRay = Camera.main.ScreenPointToRay(screenPos);

        if(Physics.Raycast(testRay, out hit, RAY_THRES))
        {
            if (hit.collider.gameObject.GetInstanceID() == obj.GetInstanceID())
                return true;
        }

        return false;
    }
}
