using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActive : MonoBehaviour {

    public float trappedTime = 3f;
    private MovingObject victim;
    public AudioClip TrapCatchedAudioClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        victim = other.gameObject.GetComponent<MovingObject>();
        if (victim && !victim.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<MovingObject>().MoveTime = 100000;
            Invoke("ReleaseTrap", trappedTime);
            this.GetComponent<AudioSource>().PlayOneShot(TrapCatchedAudioClip);
        }
    }

    void ReleaseTrap()
    {
        victim.SetDefaultSpeed();
        Destroy(this.gameObject);
    }
}
