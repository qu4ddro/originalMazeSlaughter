using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public List<Items> Inventory = new List<Items>();
    public int InventorySize;

    public GameObject Trap;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        int horizontal = 0; //Used to store the horizontal move direction.
        int vertical = 0; //Used to store the vertical move direction.

        // Get Inputs
        horizontal = (int) (Input.GetAxisRaw("Horizontal"));
        vertical = (int) (Input.GetAxisRaw("Vertical"));

        //Avoid diagonal Movement
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove(horizontal, vertical);
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
        if (Input.GetKeyDown("2"))
        {
            UseTrap();
        }
        if (Input.GetKeyDown("e"))
        {
            UseSwitch();
        }
    }

    protected override void OnCantMove(GameObject hitted)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        switch (other.tag)
        {
            case "Exit":
                if (Inventory.OfType<Key>().Any())
                {
                    this.transform.Find("Key").GetComponent<Items>().PlayUseSound();
                    GameManager.instance.NextLevel();
                }
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
    private void UseTrap()
    {
        if (Inventory.OfType<Trap>().Any())
        {
            var trap = Inventory.First(select => select.GetType() == typeof(Trap)) as Trap;
            Inventory.Remove(trap);
            transform.Find("Trap").gameObject.SetActive(false);
            Instantiate<GameObject>(Trap, this.transform.position, Quaternion.identity);

        }
    }

    private void UseSwitch()
    {
        var hit = CastInCurrentDirection();
        var hittedGameObject = hit.rigidbody.gameObject.GetComponent<Switch>();
        if (hittedGameObject.GetType() == typeof(Switch))
        {
            hittedGameObject.Activate();
        }
    }

    private void UseAxe()
    {
        if (Inventory.OfType<Axe>().Any())
        {
            var axe = Inventory.First(select => select.GetType() == typeof(Axe)) as Axe;
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
                        Inventory.Remove(axe);
                        transform.Find("Axe").gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public override void Die()
    {
        GameManager.instance.GameOver();    
    }
}