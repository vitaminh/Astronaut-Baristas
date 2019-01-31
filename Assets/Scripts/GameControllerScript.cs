using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public OrderControllerScript orderController;

    public Text gameTimeText;   // text to display game time

    private float gameTime;     // game playing time
    private bool isRunning;     // is the game running?
    private bool isFinished;    // is the game finished?
    private CharacterController[] players;  // all player character controllers

	// Use this for initialization
	void Start () {
        isRunning = isFinished = false;
        players = FindObjectsOfType<CharacterController>(); // get all player character controllers
        foreach(CharacterController c in players)           // then disable them
        {
            c.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            gameTime -= Time.deltaTime;
            gameTimeText.text = "Time: " + ((int)gameTime).ToString();
            if(gameTime <= 0)
            {
                EndGame();
            }
        }
	}

    // starts the game
    void StartGame()
    {
        gameTime = 120.0f;
        isRunning = true;
        isFinished = false;
        foreach (CharacterController c in players)           // enable player controls
        {
            c.enabled = true;
        }
        orderController.StartOrders();  // start creating customer orders
    }

    // ends the game
    void EndGame()
    {
        gameTimeText.text = "Time: 0";
        isRunning = false;
        isFinished = true;
        foreach (CharacterController c in players)           // disable player controls
        {
            c.enabled = false;
        }
        orderController.StopOrders();   // stop customer orders
    }

    //This section creates the Graphical User Interface (GUI)
    void OnGUI()
    {

        if (!isRunning)
        {
            string message;
            string exitMessage = "Exit Game";
            Rect startButton = new Rect(Screen.width / 2 - 70, Screen.height / 4, 140, 30);
            Rect exitButton = new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 30);

            if (isFinished)
            {
                message = "Reload Game";
                if (GUI.Button(startButton, message))
                {
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                if (GUI.Button(exitButton, exitMessage))
                {
                    Application.Quit();
                }
            }
            else
            {
                message = "Click to Play";
                if (GUI.Button(startButton, message))
                {
                    StartGame();
                }
                if (GUI.Button(exitButton, exitMessage))
                {
                    Application.Quit();
                }
            }
        }
    }
}
