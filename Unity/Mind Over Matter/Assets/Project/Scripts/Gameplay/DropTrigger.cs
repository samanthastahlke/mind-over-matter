using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
DropTrigger.cs
DropTrigger (c) Ominous Games 2017
*/

public class DropTrigger : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        MindObject obj = c.collider.GetComponent<MindObject>();

        if (obj != null)
        {
            obj.LoseFocus();
        }
    }
}
