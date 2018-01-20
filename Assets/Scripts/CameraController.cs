﻿using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraController : MonoBehaviour
{

    private GameObject Player;

    // Use this for initialization 
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        if(Player)
            transform.position = Player.transform.position - new Vector3(0, 0, 1);
    }
}