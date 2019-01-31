using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour {

    public List<GameObject> itemsToBeProcessed;
    public List<GameObject> processedItems;

// Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual bool CanProcess(GameObject x) {
        // hack to remove "(clone)" from end of created object's name
        string tempName = GetObjectName(x);
        foreach (GameObject obj in itemsToBeProcessed)
        {
            if(obj.name == tempName)
            {
                print("Can process " + tempName);
                return true;
            }
        }
        print("Can't process " + tempName);
        return false;
    }

    public virtual GameObject ProcessItem( GameObject x)
    {
        // hack to remove "(clone)" from end of created object's name
        string temp = GetObjectName(x);
        int index = 0;
        bool hasItemToReturn = false;
        for(int i = 0; i < itemsToBeProcessed.Count; i++)
        {
            if(itemsToBeProcessed[i].name == temp)
            {
                index = i;
                hasItemToReturn = true;
            }
        }
        if (hasItemToReturn)
        {
            return Instantiate(processedItems[index]);
        }
        else
        {
            return null;
        }
    }

    // helper method to separate "(Clone)" from GameObject names
    protected string GetObjectName(GameObject x)
    {
        string[] temp = x.name.Split(new char[] { '(' });
        string tempName = temp[0].Trim();
        return tempName;
    }
}
