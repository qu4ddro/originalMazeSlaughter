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
            BreakableWall wall = hitted.gameObject.GetComponent<BreakableWall>();
            wall.DamageWall(100);
            this.gameObject.SetActive(false);
        }
        else if (hitted.gameObject.GetComponent<MovingObject>())
        {
            hitted.gameObject.GetComponent<MovingObject>().Die();
        }
    }
}
