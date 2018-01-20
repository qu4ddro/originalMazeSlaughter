using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioClip AmbientAudioClip;

    public AudioClip StartAudioClip;

    public AudioClip[] RandomAudioClips;

    private AudioSource myAudioSource;
	// Use this for initialization
	void Start ()
	{
	    myAudioSource = this.GetComponent<AudioSource>();
	    myAudioSource.clip = AmbientAudioClip;
	    myAudioSource.Play();

        Invoke("PlayDelayedAudio",2f);
	}

    private void PlayDelayedAudio()
    {
        myAudioSource.PlayOneShot(StartAudioClip);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
