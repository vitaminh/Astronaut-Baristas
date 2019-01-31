using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ServiceCounter : Processor {

    public Text coffeeServedText;
    public OrderControllerScript orderController;   // reference to order generator
    public GameObject dirtyMugSpawnLocation;        // where dirty mugs will drop from

    private int totalScore;         // score accumulated
    private float dirtyDishTimer;    // how much time before next dirty mug drops
    private Queue<string> dirtyDishQueue;   // queue of dirty dishes to process

	// Use this for initialization
	void Start () {
        totalScore = 0;
        dirtyDishTimer = 0;
        dirtyDishQueue = new Queue<string>();
        SetScoreText();
    }
	
	// Update is called once per frame
	void Update () {

		if (dirtyDishTimer > 0)      // check if dirty dish timer is running
        {
            dirtyDishTimer -= Time.deltaTime;
            if(dirtyDishTimer <= 0)  // if dirty mug timer has elapsed
            {
                string temp = dirtyDishQueue.Dequeue(); // name of dirty dish to process
                int index;                              // index of item to instantiate
                if (temp == "FilledMug")
                {
                    index = 0;
                }
                else if (temp == "ToastedPastry")
                {
                    index = 1;
                }
                else { index = 0; }
                GameObject newDirtyDish = Instantiate(processedItems[index]);   // create new dish
                newDirtyDish.transform.position = dirtyMugSpawnLocation.transform.position;  // move new mug to spawn location
                Rigidbody rb = newDirtyDish.GetComponent<Rigidbody>();
                rb.useGravity = true;  // object subject to laws of physics
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }
        }
        else if(dirtyDishQueue.Count > 0) // if timer has elapsed and there is another mug to process
        {
            dirtyDishTimer = Random.Range(8.0f, 12.0f);  // start the timer with random value
            print("Dirty Dish Timer: " + dirtyDishTimer);
        }
        else {
            // do nothing if no timer is running and there are no dirty mugs to process
        }
	}

    public override bool CanProcess(GameObject x)
    {
        // hack to remove "(clone)" from end of created object's name
        string tempName = GetObjectName(x);
        foreach (GameObject obj in itemsToBeProcessed)
        {
            if (obj.name == tempName)
            {
                if (orderController.HasOrder(tempName))
                {
                    print("Can process " + tempName);
                    return true;
                }
            }
        }
        print("Can't process " + tempName);
        return false;
    }

    public override GameObject ProcessItem(GameObject x)
    {
        string temp = GetObjectName(x);
        totalScore += orderController.CompleteAndScoreOrder(temp);  // add score
        dirtyDishQueue.Enqueue(temp);   // add dirty dish to queue
        SetScoreText();     // update outputed score
        return null;        // will only accept filled mugs, so return null
    }

    void SetScoreText() {
        coffeeServedText.text = "Score: " + totalScore.ToString();
    }
}
