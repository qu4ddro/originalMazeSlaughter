using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Items : MonoBehaviour
{
    public AudioClip PickupItemAudioClip;
    public AudioClip UseItemAudioClip;

    private AudioSource myAudioSource;

    public void Awake()
    {
        myAudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void Start()
    {
        myAudioSource.PlayOneShot(PickupItemAudioClip);
    }
}