﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public string[] items = new string[3];

    Animator animator;

    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        //Get a component reference to the Player's animator component
        // animator = GetComponent<Animator>();

        //Call the Start function of the MovingObject base class.
        base.Start();

        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Anim
        animator.SetInteger("_horizontal", horizontal);
        animator.SetInteger("_vertical", vertical);

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {

            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }

        //Schritte-Sounds
        if (isMoving)
        {
            GetComponent<AudioSource>().UnPause();
        }
        else
        {
            GetComponent<AudioSource>().Pause();
        }

     

    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);
    }


    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        Debug.Log("oncantmove");
    }


    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Disable the player object since level is over.
            enabled = false;
            if (hasItem("Key"))
                GameManager.instance.NextLevel();
            else
            {
                Debug.Log("No Key! Can't Escape");
            }
        }
        else if (other.tag == "Key" || other.tag == "Axe" || other.tag == "Trap" || other.tag == "Torch" || other.tag == "Syringe" || other.tag == "Key")
        {
            PickupItem(other);
        }
    }

    public bool hasItem(string searchedItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == searchedItem)
                return true;
        }
        return false;
    }

    private void PickupItem(Collider2D pickedUpItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == "")
            {
                items[i] = pickedUpItem.gameObject.tag;
                Destroy(pickedUpItem.gameObject);
                return;
            }
        }
    }

}