using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

    private bool isHeld;    // is this object already being held?

	// Use this for initialization
	void Start () {
        isHeld = false;
	}

    // Update is called once per frame
    void Update() {

    }

    // set isHeld to detect held status
    void Held( bool heldStatus ) {
        isHeld = heldStatus;
    }
    // get isHeld status
    public bool GetHeldStatus()
    {
        return isHeld;
    }
}
