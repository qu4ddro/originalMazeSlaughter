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

    public GameObject TrapActive;

    private SoundController SoundManager;

    public float hitTimeOut = 1.0f;

    private float lasthit;

    protected override void Start()
    {
        base.Start();
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
    }

    protected override void Update()
    {
        base.Update();

        int horizontal = 0; //Used to store the horizontal move direction.
        int vertical = 0; //Used to store the vertical move direction.

        // Get Inputs
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

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
        //Debug.Log(hitted);
        if (hitted.gameObject.CompareTag("Exit"))
        {
            if (Inventory.OfType<Key>().Any())
            {
                this.transform.Find("Key").GetComponent<Items>().PlayUseSound();
                GameManager.instance.NextLevel();
            }
            else
            {
                GameManager.instance.PlayExitErrorSound();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        switch (other.tag)
        {
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
        else
        {
            GameManager.instance.PlayItemErrorSound();
        }
    }
    private void UseTrap()
    {
        if (Inventory.OfType<Trap>().Any())
        {
            var ind = Inventory.FindIndex(sel => sel.GetType() == typeof(Trap));
            Inventory.RemoveAt(ind);
            SoundManager.PlayOneShot(transform.Find("Trap").gameObject.GetComponent<Items>().UseItemAudioClip);
            transform.Find("Trap").gameObject.GetComponent<Items>().PlayUseSound();
            Instantiate<GameObject>(TrapActive, this.transform.position, Quaternion.identity);
            transform.Find("Trap").gameObject.SetActive(false);
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
            if (Time.time - lasthit < hitTimeOut)
                return;
            var axe = Inventory.First(select => select.GetType() == typeof(Axe)) as Axe;
            audioSource.PlayOneShot(axe.UseItemAudioClip);
            //ToDo: animation
            var hit = CastInCurrentDirection();
            if (!hit)
                return;
            BreakableWall hittedGameObject = hit.rigidbody.gameObject.GetComponent<BreakableWall>();
            if (hittedGameObject != null)
            {
                hittedGameObject.DamageWall(1);
                axe.Health--;
                lasthit = Time.time;
                if (axe.Health > 0)
                {
                }
                else
                {
                    var ind = Inventory.FindIndex(sel => sel.GetType() == typeof(Axe));
                    Inventory.RemoveAt(ind);
         
                    //Inventory.Remove(axe);
                    transform.Find("Axe").gameObject.SetActive(false);
                }
            }
        }
    }

    public override void Die()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.GameOver();
    }
}