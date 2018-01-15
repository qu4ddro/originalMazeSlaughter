using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float Multiplier;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<MovingObject>().MoveTime = other.gameObject.GetComponent<MovingObject>().MoveTime * Multiplier;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<MovingObject>().MoveTime = other.gameObject.GetComponent<MovingObject>().MoveTime / Multiplier;
        }

    }
}
