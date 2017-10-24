using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class menuContext : MonoBehaviour {

    public GameObject observer;
    public float width;
    public float height;
    public bool hasbg = true;
    public GameObject background;
    public GameObject textClone;
    public List<GameObject> contexts;
    public GameObject dynamicContext;
    SteamVR_Controller.Device device;

    // Use this for initialization
    void Start ()
    {
        observer = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        if (width == 0)
            width = 0.75f;
        if (height == 0)
            height = 0.2f;
        contexts = new List<GameObject>();
        if(hasbg)
            background.transform.localScale = new Vector3(width, height, 1);

    }
	
	// Update is called once per frame
	void Update () {
    }

    public void addContext(string message) {
        GameObject ctx = GameObject.Instantiate<GameObject>(textClone, transform);
        contexts.Add(ctx);
        for (var i = contexts.Count - 1; i >= 0; i--)
            contexts[i].transform.localPosition = new Vector3(0, -0.1f * i + (contexts.Count - 1) * 0.05f, 0);
        ctx.GetComponent<TextMesh>().text = message;
        height += 0.1f;
        if (hasbg)
            background.transform.localScale = new Vector3(width, height, 1);

    }
}
