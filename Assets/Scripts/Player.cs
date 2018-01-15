using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEditor;
using UnityEngine.UI; //Allows us to use UI.
using UnityEngine.SceneManagement;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public List<Items> Inventory = new List<Items>();
    public int InventorySize;

    private void Update()
    {
        int horizontal = 0; //Used to store the horizontal move direction.
        int vertical = 0; //Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int) (Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int) (Input.GetAxisRaw("Vertical"));

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

        if (Input.GetKeyDown("1"))
        {
            UseAxe();
        }
    }

    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
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
                if (Inventory.Contains(new Key()))
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

    private void PickupItem(Collider2D pickedUpItem)
    {
        if (Inventory.Count < InventorySize)
        {
            Destroy(pickedUpItem.gameObject);
            switch (pickedUpItem.tag)
            {
                case "Key":
                    transform.Find("Key").gameObject.SetActive(true);
                    Inventory.Add(new Key());
                    break;
                case "Axe":
                    transform.Find("Axe").gameObject.SetActive(true);
                    Inventory.Add(new Axe());
                    break;
                case "Trap":
                    transform.Find("Trap").gameObject.SetActive(true);
                    Inventory.Add(new Trap());
                    break;
            }
        }
    }

    private void UseAxe()
    {
        if (Inventory.OfType<Axe>().Any())
        {
            var axe = Inventory.First(select => select.GetType() == typeof(Axe)) as Axe;
            Inventory.Remove(axe);
            //ToDo: animation
            var hit = CastInCurrentDirection();
            var hittedGameObject = hit.rigidbody.gameObject.GetComponent<BreakableWall>();
            if (hittedGameObject.GetType() == typeof(BreakableWall))
            {
                hittedGameObject.DamageWall(1);

                if (axe != null)
                {
                    axe.Health--;
                    if (axe.Health >0)
                    {
                        Inventory.Add(axe);
                    }
                    else
                    {
                        transform.Find("Axe").gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}