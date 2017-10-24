using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class laserpointer : MonoBehaviour {

    public float laserthickness = 0.5f;
    public GameObject menuLatch;
    public GameObject laser;
    public GameObject pointer;
    internal GameObject selecting;
    internal bool locked;
    internal menuContext selInfo;
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Ray r = new Ray();
        r.origin = this.transform.position;
        r.direction = this.transform.parent.rotation * Vector3.forward;
        RaycastHit[] cols = Physics.RaycastAll(r);
        RaycastHit? closest = null;
        foreach (RaycastHit rc in cols) {
            if (rc.distance < 1) continue;
            if (rc.collider.gameObject.GetComponent<selectableInfo>() == null) continue;
            if (closest == null) closest = rc;
            if (rc.distance < closest.Value.distance)
                closest = rc;
        }

        if (closest.HasValue) {
            laser.transform.localScale = new Vector3(laserthickness, laserthickness, closest.Value.distance);
            laser.transform.localPosition = new Vector3(0, 0, closest.Value.distance / 2);
            pointer.transform.position = closest.Value.point;
            selecting = closest.Value.collider.gameObject;
        }
        else {
            float longdist = 10000;
            laser.transform.localScale = new Vector3(laserthickness / 2, laserthickness / 2, longdist);
            laser.transform.localPosition = new Vector3(0, 0, longdist / 2);
            pointer.transform.localPosition = new Vector3();
            selecting = null;
        }

        if (selecting != null) {
            if (selInfo == null)
                selInfo = selecting.GetComponent<selectableInfo>().createMenu(menuLatch);
        }
        else {
            if (!locked && selInfo != null) {
                GameObject.Destroy(selInfo.gameObject);
                selInfo = null;
            }
        }
	}
}
