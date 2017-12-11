using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MovingObject
{

    private int _horizontal = 0;
    private int _vertical = 0;
    private DateTime time;
    public int KillerMovementTimeInMS;



    // Use this for initialization
    void Start()
    {
        base.isMoving = false;
        time = DateTime.Now;
        CheckDirectionAndSetMovement();
        base.Start();
    }

    private void CheckDirectionAndSetMovement()
    {
        if (direction == Direction.North)
        {
            _horizontal = 0;
            _vertical = 1;
        }
        if (direction == Direction.East)
        {
            _horizontal = 1;
            _vertical = 0;
        }
        if (direction == Direction.South)
        {
            _horizontal = 0;
            _vertical = -1;
        }
        if (direction == Direction.West)
        {
            _horizontal = -1;
            _vertical = 0;
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Debug.Log("killer_oncantmove");
        //ChangeDirectionToRight();
    }

    private void ChangeDirectionToRight()
    {
        if (direction == Direction.North)
        {
            direction = Direction.East;
            _horizontal = 1;
            _vertical = 0;
        }
        else if (direction == Direction.East)
        {
            direction = Direction.South;
            _horizontal = 0;
            _vertical = -1;
        }
        else if (direction == Direction.South)
        {
            direction = Direction.West;
            _horizontal = -1;
            _vertical = 0;
        }
        else if (direction == Direction.West)
        {
            direction = Direction.North;
            _horizontal = 0;
            _vertical = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CheckDirectionAndSetMovement();
        var deltatime = (DateTime.Now - time).Milliseconds;
        if (!base.isMoving && deltatime >= KillerMovementTimeInMS)
        {
            //rechts
            ChangeDirectionToRight();
            var hit = TryMove(_horizontal, _vertical);
            if (hit.transform == null)
            {
                MoveKiller();
                time = DateTime.Now;
                return;
            }
            //geradeaus
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            hit = TryMove(_horizontal, _vertical);
            if (hit.transform == null)
            {
                MoveKiller();
                time = DateTime.Now;
                return;
            }
            //links
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            hit = TryMove(_horizontal, _vertical);
            if (hit.transform == null)
            {
                MoveKiller();
                time = DateTime.Now;
                return;
            }
            //hinten
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            ChangeDirectionToRight();
            hit = TryMove(_horizontal, _vertical);
            if (hit.transform == null)
            {
                MoveKiller();
                time = DateTime.Now;
                return;
            }
        }



    }

    private void MoveKiller()
    {
        //Check if moving horizontally, if so set vertical to zero.
        if (_horizontal != 0)
        {
            _vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (_horizontal != 0 || _vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(_horizontal, _vertical);
            Debug.Log(direction.ToString());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Disable the player object since level is over.
            enabled = false;
        }
    }

}
