using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public List<GameObject> SubordinateGameObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        var timer = 0.1f;
        foreach (var subordinateGameObject in SubordinateGameObjects)
        {
            StartCoroutine(ActivateWater(subordinateGameObject,timer));
            timer+=0.1f;
        }
    }

    private IEnumerator ActivateWater(GameObject water, float wait)
    {
        yield return new WaitForSeconds(wait);
        water.SetActive(!water.activeSelf);
    }
}
