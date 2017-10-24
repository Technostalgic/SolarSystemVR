using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class laserTeleporter : MonoBehaviour {
    public GameObject teleportee;
    public float laserthickness = 0.005f;
    public GameObject controllerObj;
    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;
    public GameObject laser;
    public GameObject pointer;
    internal bool pretriggered;
    internal GameObject bounceTracer;
    internal GameObject bounceLander;

	// Use this for initialization
	void Start (){
        controller = controllerObj.gameObject.GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bounceLander != null)
        {
            GameObject.Destroy(bounceLander);
            bounceLander = null;
        }
        if (bounceTracer != null)
        {
            GameObject.Destroy(bounceTracer);
            bounceTracer = null;
        }

        Vector3? tpoint = null;
        Ray r = new Ray(this.transform.position, this.transform.rotation * Vector3.forward);
        RaycastHit[] rchits = Physics.RaycastAll(r);
        RaycastHit? closest = null;
        foreach (RaycastHit hit in rchits)
        {
            if (!closest.HasValue) closest = hit;
            if (closest.Value.distance > hit.distance) closest = hit;
        }
        if (closest.HasValue)
        {
            float dist = closest.Value.distance;
            laser.transform.localScale = new Vector3(laserthickness, laserthickness, dist);
            laser.transform.localPosition = new Vector3(0, 0, dist / 2f);
            pointer.transform.position = closest.Value.point;
            tpoint = closest.Value.point;

            if (closest.Value.collider.gameObject.tag == "Planet")
                tpoint = makeBounceTracer(closest.Value.point, closest.Value.normal);
            else
            {
                if (tpoint.HasValue)
                {
                    var mp = tpoint.Value;
                    if (mp.y <= 0)
                        mp.y = 0;
                    if (!bounceLander)
                        bounceLander = GameObject.Instantiate<GameObject>(pointer, mp, new Quaternion());
                    bounceLander.transform.position = mp;
                    bounceLander.transform.localScale = new Vector3(0.7f, 1.4f, 0.7f);
                }
            }
        }
        else
        {
            float longdist = 50000;
            pointer.transform.localPosition = new Vector3();
            pointer.transform.localScale = new Vector3(0, 0, 0);
            laser.transform.localScale = new Vector3(laserthickness, laserthickness, longdist);
            laser.transform.localPosition = new Vector3(0, 0, longdist / 2f);
        }
        device = SteamVR_Controller.Input((int)controller.index);
        if (device.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            if (tpoint.HasValue)
                if(!pretriggered)
                    trigger(tpoint.Value);
            pretriggered = true;
        }
        else pretriggered = false; 
    }

    Vector3 makeBounceTracer(Vector3 colpoint, Vector3 normal)
    {
        float bounceDistance = 5;
        Vector3 cpointA = colpoint;
        Vector3 cpointB = colpoint + (normal * bounceDistance);
        if (!bounceTracer)
            bounceTracer = GameObject.Instantiate<GameObject>(laser, (cpointA + cpointB) / 2, Quaternion.LookRotation(cpointB - cpointA));
        bounceTracer.transform.localScale = new Vector3(laserthickness * 2, laserthickness * 2, (cpointB - cpointA).magnitude);
        bounceTracer.transform.rotation = Quaternion.LookRotation(cpointB - cpointA);
        bounceTracer.transform.position = (cpointA + cpointB) / 2;
        if (!bounceLander)
            bounceLander = GameObject.Instantiate<GameObject>(pointer, cpointB, new Quaternion());
        bounceLander.transform.position = cpointB;
        bounceLander.transform.localScale = new Vector3(0.7f, 1.4f, 0.7f);

        return cpointB;
    }

    void trigger(Vector3 tpoint) {
        if (tpoint.y <= 0)
            tpoint.y = 0;
        teleportee.transform.position = tpoint;
    }
}
