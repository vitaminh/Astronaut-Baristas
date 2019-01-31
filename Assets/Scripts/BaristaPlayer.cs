using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaristaPlayer : MonoBehaviour {

    public string playerID = "P1";
    public float speed = 6.0f;
    public float turnRate = 5.0f;
    public float gravity = 20.0f;

    // movement variables
    private string hInput;
    private string vInput;
    private string fire1Input;
    private string fire2Input;
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float xDir;
    private float yDir;

    // item interaction variables
    private bool isWorking;     // is player processing something?
    private GameObject currentTarget;       // item player will interact with by pressing action
    private GameObject carriedItem;         // item player is currently carrying
    private PickUpItem pickUpItemExists;    // used to see if encountered object is pickup-able
    private Dispenser dispenserExists;      // used to see if player encounters dispenser
    private Processor processorExists;      // does player encounter processor
    private enum targetType { None, Item, Dispenser, Processor };    // tracks target types
    private targetType playerTarget;                // variable for holding target type
    private Transform holdingArea;          // child transform for players that tracks holding item position

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hInput = "Horizontal_" + playerID;  // allows for reuse of script for multiple players
        vInput = "Vertical_" + playerID;
        fire1Input = "Fire1_" + playerID;
        fire2Input = "Fire2_" + playerID;
        isWorking = false;
        playerTarget = targetType.None;
        holdingArea = transform.Find("HoldingArea");
    }

    void Update()
    {
        // handle picking up / putting down objects
        if (Input.GetButtonDown(fire1Input))
        {
            if (carriedItem)    // check if player is already holding something
            {
                PutDown();
            } else {
                PickUp();
            }
        }

        // handle processing objects
        if(Input.GetButtonDown(fire2Input) && carriedItem && processorExists )
        {
            print("Fire2 Pressed");
            // check if processor can process what player is carrying
            if (processorExists.CanProcess(carriedItem))
            {
                print("processor can process " + carriedItem);
                GetProcessedItem();
            }
        }

        xDir = Input.GetAxis(hInput);
        yDir = Input.GetAxis(vInput);

        if(xDir == 0 && yDir == 0)  // if player not moving
        {
            anim.SetInteger("AnimationPar", 0);
            return;
        }

        if (controller.isGrounded && !isWorking)  // begin calculating player movement
        {
            moveDirection = new Vector3(xDir, 0, yDir); // get direction of movement
            // rotate player in direction of movement
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * turnRate);
            moveDirection *= speed;
        }
        moveDirection.y -= gravity * Time.deltaTime;        // account for gravity
        controller.Move(moveDirection * Time.deltaTime);    // move character
        anim.SetInteger("AnimationPar", 1);
    }

    void SetCurrentTargetItem(Collider other) {
        // if currently holding an object, only target processors
        if (carriedItem)
        {
            if(!currentTarget)
            {
                processorExists = other.gameObject.GetComponent<Processor>();
                if(processorExists)
                {
                    currentTarget = other.gameObject;
                    playerTarget = targetType.Processor;
                    print("Current Target: " + currentTarget);
                }
            }
        }
        // if currently not targeting anything
        else if (!currentTarget)
        {
            // check if object player collides with is able to be picked up
            pickUpItemExists = other.gameObject.GetComponent<PickUpItem>();
            dispenserExists = other.gameObject.GetComponent<Dispenser>();
            processorExists = null;     // if not holding anything, target cannot be processor
            // if it is, set is as the current target
            if (pickUpItemExists)
            {
                if (!pickUpItemExists.GetHeldStatus())  // if item to be picked up is not currently being held by another player
                {
                    currentTarget = other.gameObject;   // set as current target
                    playerTarget = targetType.Item;
                    print("Current Target: " + currentTarget);
                }
            }
            else if (dispenserExists)
            { // if not check if it is a dispenser
                currentTarget = other.gameObject;   // set dispensed item as current target
                playerTarget = targetType.Dispenser;
                print("Current Target: " + currentTarget);
            }
            else
            {   // otherwise, do nothing

            }
        }
        else { }    // if player already has a target, do nothing

    }

    // set current object encountered to interact with as target
    void OnTriggerEnter(Collider other) {
        SetCurrentTargetItem(other);
    }

    // set object being collided with as target
    void OnTriggerStay(Collider other) {
        SetCurrentTargetItem(other);
    }

    // if player leaves collision range of current target, deselect current target
    void OnTriggerExit(Collider other) {
        // if current target is the collider player is exiting
        if( currentTarget = other.gameObject ) {
            // deselect it as the target
            currentTarget = null;
            playerTarget = targetType.None;
            print("Current Target: " + currentTarget);
        }
    }

    void PickUp() {
        // check is player is targeting dispenser
        if (playerTarget.Equals(targetType.Dispenser)) {
            currentTarget = dispenserExists.dispense();
        }
        GetItem();
    }

    void PutDown() {
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        rb.useGravity = true;  // object subject to laws of physics
        rb.isKinematic = false;
        rb.detectCollisions = true;
        carriedItem.transform.parent = null;    // deparent dropped item
        carriedItem = null;                     // clear reference to dropped item
        currentTarget = null;                   // clear reference to anything currently targeted
    }

    void GetItem() {
        // player must have current target
        if (currentTarget)
        {
            carriedItem = currentTarget;    // set carried item to be the current target
            currentTarget = null;           // clear out current target
            Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
            rb.useGravity = false;  // object no longer obeys laws of physics
            rb.isKinematic = true;
            rb.detectCollisions = false;
            carriedItem.transform.parent = holdingArea.parent;      // set player as object parent
            carriedItem.transform.position = holdingArea.position;  // set new object position
        }
    }

    void GetProcessedItem() {
        print("Getting Processed Item");
        currentTarget = processorExists.ProcessItem(carriedItem);
        Destroy(carriedItem);
        GetItem();
    }
}
