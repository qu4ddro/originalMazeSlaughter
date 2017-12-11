using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Experimental.UIElements;


//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
public abstract class MovingObject : MonoBehaviour
{
    public enum Direction { North, West, South, East };

    public Direction direction;
    public float moveTime = 0.1f;           //Time it will take object to move, in seconds.
    public LayerMask blockingLayer;         //Layer on which collision will be checked.
    public int moveSteps;

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;          //Used to make movement more efficient.

    public bool isMoving;



    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    protected MoveModel Move(int xDir, int yDir)
    {
        if (isMoving)
        {
            return new MoveModel { CanMove = false, IsMoving = isMoving };
        }
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //start SmoothMovement co-routine passing in the Vector2 end as destination
        //StartCoroutine(SmoothMovement(end));
        rb2D.MovePosition(end);

        //Return true to say that Move was successful
        return new MoveModel { CanMove = true }; ;
    }

    private void StepMove(Vector3 end)
    {
        var step = end / moveSteps;
        for (int i = 0; i < moveSteps; i++)
        {
           var newPostion = Vector3.MoveTowards(rb2D.position, end,moveTime);
            rb2D.MovePosition(newPostion);
        }
    }

    public RaycastHit2D TryMove(int xDir, int yDir)
    {
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        var hit = Physics2D.Linecast(start, end, blockingLayer);

        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;

        return hit;
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {

        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        var sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        isMoving = true;
        var counter = 0;
        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon || counter > 10)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            var newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            counter++;

            if (counter > 10)
            {
                rb2D.MovePosition(end);
                isMoving = false;
                yield break;
            }
            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;

        }
        isMoving = false;
    }

    //The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        ////Hit will store whatever our linecast hits when Move is called.

        ////Set canMove to true if Move was successful, false if failed.
        //var moveModel = Move(xDir, yDir);
        //if (moveModel.IsMoving)
        //{
        //    return moveModel.IsMoving;
        //}
        ////Check if nothing was hit by linecast
        //if (moveModel.Hit.transform == null)
        //    //If nothing was hit, return and don't execute further code.
        //    return false;

        ////Get a component reference to the component of type T attached to the object that was hit
        //T hitComponent = moveModel.Hit.transform.GetComponent<T>();

        ////If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        //if (!moveModel.CanMove && hitComponent != null)

        //    //Call the OnCantMove function and pass it hitComponent as a parameter.
        //    OnCantMove(hitComponent);
        //return moveModel.IsMoving;

        var hit = TryMove(xDir, yDir);
        if (hit.transform != null)
        {
            var hitComponent = hit.transform.GetComponent<T>();
            OnCantMove(hitComponent);
        }
        else
        {
            Move(xDir, yDir);
        }
    }

    //The abstract modifier indicates that the thing being modified has a missing or incomplete implementation.
    //OnCantMove will be overriden by functions in the inheriting classes.
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}