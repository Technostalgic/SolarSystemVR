using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeScaleChange : MonoBehaviour
{
    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;
    public GameObject contextMenu;

    Vector2 touchpad;

    private float sensitivityX = 1.5f;

    // Use this for initialization
    void Start()
    {
        controller = gameObject.GetComponent<SteamVR_TrackedObject>();
    }
    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)controller.index);
        //If finger is on touchpad
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //Read the touchpad values
            touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            Time.timeScale = (touchpad.x + 1) / 2;
            contextMenu.GetComponent<menuContext>().dynamicContext.GetComponent<TextMesh>().text = ((float)Mathf.Round(Time.timeScale * 100) / 100).ToString();
            contextMenu.active = true;
            //Debug.Log ("Touchpad X = " + touchpad.x + " : Touchpad Y = " + touchpad.y);
        }
        else contextMenu.active = false;
    }
}
