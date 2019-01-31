using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastryToaster : Processor {

    public GameObject plateIcon, pastryIcon; // indicators for ingredients added
    private bool hasPlate, hasPastry;

	// Use this for initialization
	void Start () {
        hasPastry = hasPlate = false;
        DisableIcons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // check if toaster can process held object
    public override bool CanProcess(GameObject x)
    {
        string tempName = GetObjectName(x);
        if(tempName == "CleanPlate" && !hasPlate)
        {
            print("Can process " + tempName);
            return true;
        }
        else if(tempName == "Pastry" && !hasPastry)
        {
            print("Can process " + tempName);
            return true;
        }
        else
        {
            print("Can't process " + tempName);
            return false;
        }
    }

    // process held object
    public override GameObject ProcessItem(GameObject x)
    {
        // hack to remove "(clone)" from end of created object's name
        string tempName = GetObjectName(x);
        print("tempName: " + tempName);

        // check if we are adding a new ingredient to the toaster
        if (tempName == "CleanPlate")
        {
            hasPlate = true;
            print("Clean Plate added");
            plateIcon.SetActive(true);
        }
        else if (tempName == "Pastry")
        {
            hasPastry = true;
            print("Pastry added");
            pastryIcon.SetActive(true);
        }
        else { }    // do nothing

        if (hasPlate && hasPastry)
        {
            // empty out the machine and return a full mug
            hasPlate = hasPastry = false;
            DisableIcons();
            return Instantiate(processedItems[0]);
        }
        else
        {
            return null;
        }
    }

    void DisableIcons()
    {
        plateIcon.SetActive(false);
        pastryIcon.SetActive(false);
    }
}
