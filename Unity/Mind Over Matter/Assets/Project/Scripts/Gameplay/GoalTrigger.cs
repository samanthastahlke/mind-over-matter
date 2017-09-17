using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
GoalTrigger.cs
GoalTrigger (c) Ominous Games 2017
*/

[RequireComponent(typeof(Collider))]
public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        MindObject obj = c.GetComponent<MindObject>();

        if(obj != null)
        {
            obj.FixPosition(transform.position);
            MainHUD.instance.TriggerWin();
        }
    }
}
