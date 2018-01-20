using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Items : MonoBehaviour
{
    public AudioClip StartItemAudioClip;
    public AudioClip UseItemAudioClip;

    protected AudioSource myAudioSource;

    public void Awake()
    {
        myAudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void Start()
    {
        myAudioSource.PlayOneShot(StartItemAudioClip);
    }

    public void PlayUseSound()
    {
        myAudioSource.PlayOneShot(UseItemAudioClip);
    }
}