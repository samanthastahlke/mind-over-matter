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
    private static AppManager app;
    public GameObject worldText;

    void Awake()
    {
        if (app == null)
            app = AppManager.instance;
    }
    void OnCollisionEnter(Collision c)
    {
        MindObject obj = c.collider.GetComponent<MindObject>();

        if(obj != null)
        {
            obj.FixPosition(transform.position);
            MainHUD.instance.TriggerWin();
            ((GameplayState)app.state).TriggerWinState();

            if (worldText)
                worldText.SetActive(false);
        }
    }
}
