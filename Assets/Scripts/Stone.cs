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
        AttemptMove<Wall>(_horizontal,_vertical);
    }

    protected override void OnCantMove<T>(T component)
    {
        if (component.GetType() == typeof(BreakableWall))
            Destroy(component.gameObject);
        Destroy(this.gameObject);
    }
}
