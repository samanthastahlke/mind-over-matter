using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
EyeCaster.cs
EyeCaster (c) Ominous Games 2017
*/

[RequireComponent(typeof(GraphicRaycaster))]
public class EyeCaster : MonoBehaviour
{
    private OGInput input;
    private GraphicRaycaster raycaster;
    private EventSystem events;

    public float maxPointerSpeed = 800.0f;
    public float pointerDamp = 0.25f;
    private Vector3 pointerSpeed = Vector3.zero;
    private Vector3 lastFramePointer = Vector3.zero;
    private static AppManager app;

    void OnEnable()
    {
        app = AppManager.instance;
        input = OGInput.instance;

        raycaster = GetComponent<GraphicRaycaster>();
        events = EventSystem.current;

        if (input.trackingInputType == OGInput.TrackingInputType.TOBII && app.settings.eyeMenus)
            raycaster.enabled = false;
        else
        {
            raycaster.enabled = true;
            this.enabled = false;
        }
    }
    
    void Update() 
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Vector3.SmoothDamp(lastFramePointer, input.GetTrackingPosition(), ref pointerSpeed, pointerDamp, maxPointerSpeed);
        lastFramePointer = ped.position;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(ped, results);

        if (results.Count > 0 && (events.currentSelectedGameObject == null
            || events.currentSelectedGameObject.GetInstanceID() != results[0].gameObject.GetInstanceID()))
        {
            StartCoroutine(SwitchSelected(results[0].gameObject));
        }
        else if(results.Count == 0)
            events.SetSelectedGameObject(null);

        if (events.currentSelectedGameObject != null)
        {
            Button btn = events.currentSelectedGameObject.GetComponent<Button>();

            if (btn != null && input.StrongBlinkDown())
                btn.onClick.Invoke();

            Toggle toggle = events.currentSelectedGameObject.GetComponent<Toggle>();

            if (toggle != null && input.StrongBlinkDown())
            {
                ped.clickCount = 1;
                ped.button = PointerEventData.InputButton.Left;
                toggle.OnPointerClick(ped);
                toggle.onValueChanged.Invoke(!toggle.isOn);
            }
               
        }
    }

    IEnumerator SwitchSelected(GameObject obj)
    {
        events.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        events.SetSelectedGameObject(obj);

        Button btn = obj.GetComponent<Button>();

        if (btn != null)
            btn.Select();

        Toggle toggle = obj.GetComponent<Toggle>();

        if (toggle != null)
            toggle.Select();
    }
}
