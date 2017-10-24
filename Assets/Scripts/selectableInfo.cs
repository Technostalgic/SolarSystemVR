using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectableInfo : MonoBehaviour {

    public GameObject observer;
    public bool isPlanet = true;
    public string name;
    public string mass;
    public string size;
    public string lengthDay;
    public string lengthYear;
    public string atmosphericComposition;
    public property[] miscProperties;
    public GameObject cclone;
    public static menuContext ContextCloner;

    private void Start() {
        observer = GameObject.Find("Camera (eye)");
        ContextCloner = cclone.GetComponent<menuContext>();
    }

    public menuContext createMenu(GameObject menuLatch) {
        menuContext obj = GameObject.Instantiate<menuContext>(ContextCloner, menuLatch.transform);
        obj.transform.localPosition = new Vector3(0, 0.15f, 0.32f);
        obj.transform.localRotation = Quaternion.EulerAngles(new Vector3(45.26f, 0, 0));

        if (isPlanet) {
            obj.addContext("Name: " + name);
            obj.addContext("Mass: " + mass.ToString());
            obj.addContext("Radius: " + size.ToString());
            obj.addContext("Day: " + lengthDay);
            obj.addContext("Year: " + lengthYear);
            obj.addContext("Atmos: " + atmosphericComposition);
        }
        if(miscProperties != null)
            foreach (property prop in miscProperties)
                obj.addContext(prop.name + ": " + prop.value.ToString());
        return obj;
    }

    public struct property {
        public property(string nameA, string valueA) {
            name = nameA;
            value = valueA;
        }
        public string name;
        public string value;
    }
}
