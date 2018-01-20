using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip AmbientAudioClip;
    public AudioClip StartAudioClip;
    public AudioClip[] RandomAudioClips;

    public AudioClip WinAudioClip;

    private AudioSource myAudioSource;
	// Use this for initialization
	void Start ()
	{
	    myAudioSource = this.GetComponent<AudioSource>();
	    myAudioSource.clip = AmbientAudioClip;
	    myAudioSource.volume = 0;
	    myAudioSource.Play();
	    StartCoroutine(FadeSound(1, 3f));

        Invoke("PlayDelayedAudio",2f);
	}

    public IEnumerator FadeSound(float _newVolume, float FadeDuration)
    {
        Debug.Log("I am fading to "+_newVolume);
        float currentVolume = myAudioSource.volume;
        float startTime = Time.time;
        float elapsedTime = 0;

        while (myAudioSource.volume < _newVolume)
        {
            myAudioSource.volume = Mathf.Lerp(currentVolume, _newVolume, elapsedTime/FadeDuration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }
        Debug.Log("Fade finished");
    }

public void PlayDelayedAudio()
    {
        myAudioSource.PlayOneShot(StartAudioClip);
    }

    public void PlayWinAudioClip()
    {
        myAudioSource.PlayOneShot(WinAudioClip);
    }
}
