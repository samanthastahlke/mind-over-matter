using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
ZoneTrigger.cs
ZoneTrigger (c) Ominous Games 2017
*/

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType
    {
        RED_ZONE,
        BLUE_ZONE
    };

    public ZoneType zoneType;
    private static AppManager app;

    void Awake()
    {
        if (app == null)
            app = AppManager.instance;

        GameObject backPlane = Instantiate(app.zonePlane);
        backPlane.transform.position = new Vector3(transform.position.x, transform.position.y, app.zoneDepth);
        backPlane.transform.localScale = new Vector3(transform.localScale.x * 0.1f, 1.0f, transform.localScale.y);
        backPlane.GetComponent<Renderer>().material.color = (zoneType == ZoneType.RED_ZONE) ? app.redZone : app.blueZone;
    }

    void OnTriggerEnter(Collider c)
    {
    }
}
