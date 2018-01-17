﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActive : MonoBehaviour
{
    public float trappedTime = 3f;
    private MovingObject victim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        victim = other.gameObject.GetComponent<MovingObject>();
        if (victim && !victim.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<MovingObject>().MoveTime = 100000;
            Invoke("ReleaseTrap",trappedTime);
            this.GetComponent<AudioSource>().Play();
        }
    }

    void ReleaseTrap()
    {
        Destroy(this.gameObject);
        victim.SetDefaultSpeed();
    }
}
