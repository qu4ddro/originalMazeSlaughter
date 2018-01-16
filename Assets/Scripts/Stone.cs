using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stone : MovingObject {

	// Use this for initialization
    void Start()
    {
        base.Start();
        CheckDirectionAndSetMovement();
    }

    void OnActivate()
    {
        CheckDirectionAndSetMovement();
    }

    // Update is called once per frame
	void Update ()
	{
	    KeepOnRolling();
	}

    private void KeepOnRolling()
    {
        AttemptMove(_horizontal,_vertical);
    }

    protected override void OnCantMove(GameObject hitted)
    {
        Debug.Log(hitted);
        if (hitted.GetType() == typeof(BreakableWall))
            Destroy(hitted.gameObject);
        Destroy(this.gameObject);
    }
}
