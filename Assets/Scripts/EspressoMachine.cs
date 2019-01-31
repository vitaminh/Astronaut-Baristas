using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoMachine : Processor {

    public GameObject mugIcon, beanIcon, milkIcon;  // indicators for ingredients added

    private bool hasEmptyMug, hasGroundBeans, hasSteamedMilk;

	// Use this for initialization
	void Start () {
        hasEmptyMug = hasGroundBeans = hasSteamedMilk = false;
        DisableIcons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // check if espresso machine can process held object
    public override bool CanProcess(GameObject x)
    {
        // hack to remove "(clone)" from end of created object's name
        string tempName = GetObjectName(x);
        // check if espresso machine can process item AND does not already contain an instance of the item
        if (tempName == "EmptyMug" && !hasEmptyMug)
        {
                print("Can process " + tempName);
                return true;
        }
        else if (tempName == "GroundCoffeeBeans" && !hasGroundBeans)
        {
            return true;
        }
        else if (tempName == "SteamedMilk" && !hasSteamedMilk)
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

    public override GameObject ProcessItem(GameObject x)
    {
        // hack to remove "(clone)" from end of created object's name
        string tempName = GetObjectName(x);
        print("tempName: " + tempName);

        // check if we are adding a new ingredient to the espresso machine
        if (tempName == "EmptyMug")
        {
            hasEmptyMug = true;
            print("Empty Mug added");
            mugIcon.SetActive(true);
        }
        else if (tempName == "GroundCoffeeBeans")
        {
            hasGroundBeans = true;
            print("Ground Beans added");
            beanIcon.SetActive(true);
        }
        else if (tempName == "SteamedMilk")
        {
            hasSteamedMilk = true;
            print("Steamed Milk added");
            milkIcon.SetActive(true);
        }
        else { }    // do nothing

        if (hasEmptyMug && hasGroundBeans && hasSteamedMilk)
        {
            // empty out the machine and return a full mug
            hasEmptyMug = hasGroundBeans = hasSteamedMilk = false;
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
        mugIcon.SetActive(false);
        beanIcon.SetActive(false);
        milkIcon.SetActive(false);
    }
}
