using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
RespawnTrigger.cs
RespawnTrigger (c) Ominous Games 2017
*/

public class RespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        MindObject obj = c.GetComponent<MindObject>();

        if (obj != null)
            obj.Respawn();
    }
}
