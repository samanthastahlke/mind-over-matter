using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
LevelSelectTrigger.cs
LevelSelectTrigger (c) Ominous Games 2017
*/

public class LevelSelectTrigger : MonoBehaviour
{
    private static AppManager app;

    void Awake()
    {
        if (app == null)
            app = AppManager.instance;
    }

    void OnCollisionEnter(Collision c)
    {
        LevelSelectObject obj = c.collider.GetComponent<LevelSelectObject>();

        if (obj != null)
            SelectLevel(obj);
    }

    void OnTriggerEnter(Collider c)
    {
        LevelSelectObject obj = c.GetComponent<LevelSelectObject>();

        if (obj != null)
            SelectLevel(obj);
    }

    void SelectLevel(LevelSelectObject obj)
    {
        ((LevelSelectState)app.state).QueueLevel(obj.levelSelect);
    }
}
