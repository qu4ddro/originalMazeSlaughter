using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Wall
{

    public List<GameObject> SubordinateGameObjects;

    private DateTime cooldown = DateTime.Now;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        if ((DateTime.Now-cooldown).Seconds > 5)
        {
            var timer = 0.1f;
            foreach (var subordinateGameObject in SubordinateGameObjects)
            {
                if (subordinateGameObject.CompareTag("Water"))
                {
                    StartCoroutine(ActivateWater(subordinateGameObject, timer));
                    timer += 0.1f;
                }
                else if (subordinateGameObject.CompareTag("Stone"))
                {
                    subordinateGameObject.SetActive(true);
                }

            }
            cooldown = DateTime.Now;
        }
        
    }

    private IEnumerator ActivateWater(GameObject water, float wait)
    {
        yield return new WaitForSeconds(wait);
        water.SetActive(!water.activeSelf);
    }
}
