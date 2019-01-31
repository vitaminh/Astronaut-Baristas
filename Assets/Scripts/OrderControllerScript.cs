using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OrderControllerScript : MonoBehaviour {

    public Text orderDisplay;                        // GUI slots to display orders

    private static List<string> possibleOrderNames = new List<string>() { "FilledMug", "ToastedPastry" };    // names of possible orders
    private static System.Random rnd = new System.Random(); // random number generator
    private float initialTime = 50.0f;                      // default time to complete order
    private float timeToNextOrder;                          // time until next order is placed
    private List<CustomerOrder> listOfOrders = new List<CustomerOrder>();   // list containing all currently active orders
    private bool ordersAreComingIn;                         // is the store open?
    private int maxNumOfActiveOrders;                       // max number of active orders we have simultaneously
    
    // Use this for initialization
    void Start() {
        ordersAreComingIn = false;  // store begins closed
        timeToNextOrder = 0;        // no order countdown yet 
        maxNumOfActiveOrders = 3;   // set max # of active orders
    }

    // Update is called once per frame
    void Update() {
        if(ordersAreComingIn)                               // if store is open
        {
            // if countdown timer to new order is 0 AND the order queue is not yet full
            if (timeToNextOrder <= 0 && NeedNewOrder())
            {
                timeToNextOrder = (float)rnd.Next(8, 12);    // start a new timer
            }
            else if ( timeToNextOrder > 0 ) {        // otherwise, if timer exists, decrement it
                timeToNextOrder -= Time.deltaTime;  // decrement timer
            }
            else { }    // if no timer and don't need to make one, do nothing

            if( timeToNextOrder <= 0 && NeedNewOrder() )    // if timer has elapsed and new order required
            {
                AddOrderToList( NewCustomerOrder() );       // add a new order
            }

            UpdateOrderGUI();       // update order display
            UpdateOrderTimers();    // update timers for all orders
        }
    }

    internal void StartOrders()
    {
        ordersAreComingIn = true;                // open the store
        AddOrderToList( NewCustomerOrder() );    // add the first order of the day
    }

    internal void StopOrders()
    {
        ordersAreComingIn = false;
    }

    bool NeedNewOrder()
    {
        return listOfOrders.Count < maxNumOfActiveOrders;
    }

    void AddOrderToList( CustomerOrder order )
    {
        if( listOfOrders.Count < maxNumOfActiveOrders)
        {
            listOfOrders.Add(order);
            print("Order Added: " + order.orderName);
        }
    }

    void UpdateOrderGUI()   // update the order text
    {
        string temp = "Orders : Value" + System.Environment.NewLine;    // temp storage for string to be built
        foreach(CustomerOrder c in listOfOrders)
        {   // combine all strings
            temp = string.Concat(temp, c.displayName + ": " + (int)c.orderTimer + System.Environment.NewLine);
        }
        orderDisplay.text = temp;
    }

    void UpdateOrderTimers()    // update all order timers
    {
        foreach(CustomerOrder c in listOfOrders)
        {   // decrement each timer in active orders
            c.orderTimer -= Time.deltaTime;
            if(c.orderTimer < 0)  // limit how low the order timer can go  
            {
                c.orderTimer = 0;
            }
        }
    }

    CustomerOrder NewCustomerOrder()
    {
        int index = rnd.Next(0, possibleOrderNames.Count);   // get random item to order
        string orderName = possibleOrderNames[index];
        string displayName;
        float orderTime = initialTime;
        if (index == 0)
        {
            displayName = "Coffee";
        }
        else if(index == 1)
        {
            displayName = "Pastry";
            orderTime -= 5.0f;
        }
        else { displayName = null; }    // if no order, display no name
        return new CustomerOrder(orderName, displayName, orderTime);     // initialize and return
    }

    // check if item is current being ordered
    public bool HasOrder( string x )
    {
        foreach( CustomerOrder c in listOfOrders )
        {
            if (c.orderName == x)
            {
                return true;
            }
        }
        return false;
    }

    // after order delivered, remove order from list and return points earned
    public int CompleteAndScoreOrder( string x )
    {
        CustomerOrder temp = null;
        int score = 20;
        foreach (CustomerOrder c in listOfOrders)
        {
            int tempScore = (int)c.orderTimer;
            if (c.orderName == x && tempScore <= score) // if name matches and timer is the oldest timer running
            {
                temp = c;
                score = tempScore;
            }
        }
        listOfOrders.Remove(temp);  // remove order from list
        return score;
    }

    // custom class to hold Customer Orders and the relevant info
    class CustomerOrder
    {
        internal string orderName;
        internal string displayName;
        internal float orderTimer;

        internal CustomerOrder( string name, string dName, float time )
        {
            orderName = name;
            displayName = dName;
            orderTimer = time;
        }
    }
}
