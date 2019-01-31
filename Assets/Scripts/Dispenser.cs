using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour {

    public GameObject dispensedItem;    // type of object this dispenser provides

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // dispenses item
    public GameObject dispense() {
        GameObject newDispensedItem = Instantiate(dispensedItem);
        return newDispensedItem;
    }
}
