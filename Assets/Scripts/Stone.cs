using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stone : MovingObject {

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        CheckDirectionAndSetMovement();
    }

    void OnActivate()
    {
        CheckDirectionAndSetMovement();
    }

    // Update is called once per frame
	protected override void Update ()
	{
        base.Update();
	    KeepOnRolling();
	}

    private void KeepOnRolling()
    {
        AttemptMove(_horizontal,_vertical);
    }

    protected override void OnCantMove(GameObject hitted)
    {
        if (hitted.gameObject.GetComponent<BreakableWall>())
        {
            Destroy(hitted.gameObject);
            Destroy(this.gameObject);
        }
        else if (hitted.gameObject.GetComponent<MovingObject>())
        {
            hitted.gameObject.GetComponent<MovingObject>().Die();
        }
    }

    private void OnDestroy()
    {

    }
}
