using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class planetSelection : MonoBehaviour {

    SteamVR_TrackedObject controller;
    SteamVR_Controller.Device device;
    public GameObject user;
    public GameObject controllerObj;
    public List<GameObject> planets;
    public GameObject dynamicContext;
    internal int selection;
    internal float _anim;
    internal float _scaleAnim;
    internal float _closeAnim;
    public GameObject selected { get { return planets[selection]; } }
    internal float xFingerPos;

	// Use this for initialization
	void Start() { 
        controller = controllerObj.gameObject.GetComponent<SteamVR_TrackedObject>();

        planets = new List<GameObject>();
        for(var i = 0; i < transform.childCount; i++) {
            planets.Add(transform.GetChild(i).gameObject);
        }
        dynamicContext.GetComponent<TextMesh>().text = planets[selection].GetComponent<planetSelector>().name;
        selected.GetComponent<planetSelector>().highlight.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        float dist = 0.2f;
        foreach (GameObject planet in planets) planet.SetActive(false);
        for (var i = -2; i <= 2; i++) {
            GameObject p = planets[wrappedIndex(selection, i, planets.Count)];
            p.SetActive(true);
            float ang = (i + 1) * (Mathf.PI / 2.5f) + 0.3f;
            ang += _anim * (Mathf.PI / 2.5f);
            Vector2 spos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * dist;

            p.transform.localPosition = new Vector3(spos.x, spos.y, 0);
        }

        _anim /= 1.25f;
        device = SteamVR_Controller.Input((int)controller.index);
        this.transform.localScale = new Vector3(_scaleAnim, _scaleAnim, _scaleAnim);
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            _closeAnim = 1;
            Vector2 touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (xFingerPos == -20)
                xFingerPos = touchpad.x;

            var touchiness = 0.3f;
            if (touchpad.x > xFingerPos + touchiness)
            {
                next();
                xFingerPos += touchiness;
            }
            if (touchpad.x < xFingerPos - touchiness)
            {
                previous();
                xFingerPos -= touchiness;
            }

            if (_scaleAnim < 1)
                _scaleAnim = _scaleAnim * 1.3f + 0.01f;
            else _scaleAnim = 1;
            dynamicContext.SetActive(true);
            if (device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                select();
        }
        else
        {
            xFingerPos = -20;
            _closeAnim -= Time.deltaTime / Time.timeScale;
            if (_closeAnim <= 0)
            {
                _scaleAnim /= 1.3f;
                dynamicContext.SetActive(false);
            }
        }
	}
    int wrappedIndex(int value, int addition, int maxValue) {
        int r = value + addition;
        if (r >= 0 && r < maxValue)
            return r;
        if (r < 0) r = maxValue + r % maxValue;
        return r % maxValue;
    }

    public void next()
    {
        device.TriggerHapticPulse(500, EVRButtonId.k_EButton_SteamVR_Touchpad);
        //if (Mathf.Abs(_anim) > 0.1f) return;
        selected.GetComponent<planetSelector>().highlight.SetActive(false);
        selection++;
        if (selection >= planets.Count)
            selection = 0;
        dynamicContext.GetComponent<TextMesh>().text = planets[selection].GetComponent<planetSelector>().name;
        _anim = 1;
        selected.GetComponent<planetSelector>().highlight.SetActive(true);
    }
    public void previous()
    {
        device.TriggerHapticPulse(500, EVRButtonId.k_EButton_SteamVR_Touchpad);
        //if (Mathf.Abs(_anim) > 0.1f) return;
        selected.GetComponent<planetSelector>().highlight.SetActive(false);
        selection--;
        if (selection < 0)
            selection = planets.Count - 1;
        dynamicContext.GetComponent<TextMesh>().text = planets[selection].GetComponent<planetSelector>().name;
        _anim = -1;
        selected.GetComponent<planetSelector>().highlight.SetActive(true);
    }
    public void select() {
        float p = -25;

        switch (selected.GetComponent<planetSelector>().name)
        {
            case "Jupiter": p = -70; break;
            case "Saturn": p = -60; break;
        }
        user.transform.position = selected.GetComponent<planetSelector>().target.transform.position + new Vector3(0, 0, p);
        user.transform.parent = selected.GetComponent<planetSelector>().target.transform;
    }
}
