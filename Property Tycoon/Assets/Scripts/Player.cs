using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int currentPosition = 0;
    private int newPosition;
    private const int numSpacesOnBoard = 40;
    private bool isMoving = false;
    public Transform[] waypoints;

    public GameObject button;

    private void Start()
    {
        button = GameObject.Find("Button");
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlayerPiece();
        }
    }
    
    public void MovePlayer(int diceNum)
    {
        // Increases the players position by the value on the dice, loops back to start when greater than 40
        newPosition = (currentPosition + diceNum) % numSpacesOnBoard;
        Debug.Log("New Position: " + newPosition);
        isMoving = true;
        // Disables button when clicked
        button.SetActive(false);
    }

    public void MovePlayerPiece()
    {
        if (currentPosition != newPosition)
        {
            if (transform.position != waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[(currentPosition + 1) % numSpacesOnBoard].transform.position, 20f * Time.deltaTime);
            }
            else
            {
                currentPosition = (currentPosition + 1) % numSpacesOnBoard;
                Debug.Log("Current Position: " + currentPosition);
            }
        }
        else
        {
            isMoving = false;
            // Enables button when piece has stopped moving
            button.SetActive(true);
        }
    }

    public int GetPosition()
    {
        return currentPosition;
    }
    
    public void setWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }
    
}