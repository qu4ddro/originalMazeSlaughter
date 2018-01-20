using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioClip AmbientAudioClip;

    public AudioClip[] RandomAudioClips;

    public AudioSource AmbientAudioSource;
    public AudioSource RandomClipsAudioSource;

	// Use this for initialization
	void Start ()
	{
	    AmbientAudioSource.clip = AmbientAudioClip;
        AmbientAudioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
