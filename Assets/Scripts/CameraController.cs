using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private int height=-1;


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, height);
	}
}
