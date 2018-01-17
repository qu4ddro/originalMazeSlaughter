using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,East,South,West
}

//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;         //Layer on which collision will be checked.
    public bool isMoving;
    public Direction direction;

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;          //Used to make movement more efficient.
    private Animator animator;
    public float moveTime = 100f;           //Time it will take object to move, in milliseconds.

    protected int _horizontal = 0;
    protected int _vertical = 0;

    protected float defaultMoveTime;

    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        this.MoveTime= moveTime;

        animator = GetComponent<Animator>();

        defaultMoveTime = MoveTime;

    }

    public float MoveTime
    {
        get { return moveTime; }
        set
        {
            moveTime = value;
            inverseMoveTime = 1f / (moveTime / 1000);

        }
    }


    public void SetDefaultSpeed()
    {
        MoveTime = defaultMoveTime;
    }

    //The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected virtual void AttemptMove(int xDir, int yDir)
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = Move(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
        if (hit.transform == null)
            //If nothing was hit, return and don't execute further code.
            return;

        GameObject hitComponent = hit.transform.gameObject;

        //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        if (!canMove && hitComponent != null)

            //Call the OnCantMove function and pass it hitComponent as a parameter.
            OnCantMove(hitComponent);
    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        SetDirection(xDir, yDir);
        if (isMoving || GameManager.instance.doingSetup)
        {
            hit = new RaycastHit2D();
            
            return false;
        }

        animator.SetInteger("_horizontal", xDir);
        animator.SetInteger("_vertical", yDir);

        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null)
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            StartCoroutine(SmoothMovement(end));

            //Return true to say that Move was successful
            return true;
        }
        
        //If something was hit, return false, Move was unsuccesful.
        return false;
    }

    protected void SetDirection(int x, int y)
    {
        if (x>0)
        {
            direction = Direction.East;
        }
        if (x<0)
        {
            direction = Direction.West;
        }
        if (y>0)
        {
            direction = Direction.North;
        }
        if (y<0)
        {
            direction=Direction.South;
        }
    }

    protected void CheckDirectionAndSetMovement()
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

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        isMoving = true;

        //Anim
        animator.speed = 1;
        
        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        isMoving = false;
        animator.speed = 0;
    }

    //OnCantMove will be overriden by functions in the inheriting classes.
    protected virtual void OnCantMove(GameObject hitted)
    {
        Debug.Log(this.gameObject+" hitted :"+hitted);
    }

    public RaycastHit2D CastInCurrentDirection()
    {
        Vector2 end = gameObject.transform.position;
        switch (direction)
        {
            case Direction.North:
                end= gameObject.transform.position + Vector3.up;
                break;
            case Direction.South:
                end = gameObject.transform.position + Vector3.down;
                break;
            case Direction.East:
                end = gameObject.transform.position + Vector3.right;
                break;
            case Direction.West:
                end = gameObject.transform.position + Vector3.left;
                break;
        }
        boxCollider.enabled = false;
        var hit = Physics2D.Linecast(gameObject.transform.position, end, blockingLayer);
        boxCollider.enabled = true;
        return hit;
    }

    protected void ChangeDirectionToRight()
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

    protected void ChangeDirectionToLeft()
    {
        if (direction == Direction.North)
        {
            direction = Direction.West;
            _horizontal = -1;
            _vertical = 0;
        }
        else if (direction == Direction.East)
        {
            direction = Direction.North;
            _horizontal = 0;
            _vertical = 1;
        }
        else if (direction == Direction.South)
        {
            direction = Direction.East;
            _horizontal = 1;
            _vertical = 0;
        }
        else if (direction == Direction.West)
        {
            direction = Direction.South;
            _horizontal = 0;
            _vertical = -1;
        }
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
}