using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public string[] items = new string[3];

    private void Update()
    {
        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

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
            AttemptMove<BreakableWall>(horizontal, vertical);
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

    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        BreakableWall breakableWall = component as BreakableWall;
        breakableWall.DamageWall(1);
    }

    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        switch (other.tag)
        {
            case "Exit":
                //Disable the player object since level is over.
                enabled = false;
                if (hasItem("Key"))
                    GameManager.instance.NextLevel();
                else
                {
                    other.GetComponent<BoxCollider2D>().enabled = false;
                    Debug.Log("No Key! Can't Escape");
                    other.GetComponent<BoxCollider2D>().enabled = true;
                }

                break;
            case "Syringe":
                transform.Find("Syringe").gameObject.SetActive(true);
                Destroy(other.gameObject);
                break;
            case "Torch":
                transform.Find("Torch").gameObject.SetActive(true);
                Destroy(other.gameObject);
                break;
            case "Key":
            case "Axe":
            case "Trap":
                PickupItem(other);
                break;
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
                if (pickedUpItem.tag == "Key")
                {
                    transform.Find("Key").gameObject.SetActive(true);
                }
                else if (pickedUpItem.tag == "Axe")
                {
                    transform.Find("Axe").gameObject.SetActive(true);
                }
                else if (pickedUpItem.tag == "Trap")
                {
                    transform.Find("Trap").gameObject.SetActive(true);
                }
                return;
            }
            
        }
    }
}