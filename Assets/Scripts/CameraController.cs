﻿using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraController : MonoBehaviour
{

    public GameObject Player;
    Vector3 offset;

    // Use this for initialization 
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.position = Player.transform.position - new Vector3(0, 0, 1);
    }
}