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
    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        myAudioSource.clip = AmbientAudioClip;
        myAudioSource.volume = 0;
        myAudioSource.Play();
        StartCoroutine(FadeSound(1, 3f));

        Invoke("PlayDelayerStartClip", 2f);
    }

    public IEnumerator FadeSound(float _newVolume, float FadeDuration)
    {
        float currentVolume = myAudioSource.volume;
        float startTime = Time.time;
        float elapsedTime = 0;
        float remaining = _newVolume - currentVolume;

        while (Mathf.Abs(remaining) > float.Epsilon)
        {
            myAudioSource.volume = Mathf.Lerp(currentVolume, _newVolume, elapsedTime / FadeDuration);
            elapsedTime = Time.time - startTime;
            remaining = _newVolume - myAudioSource.volume;
            yield return null;
        }
    }

    public void PlayDelayerStartClip()
    {
        myAudioSource.PlayOneShot(StartAudioClip);
    }

    public void PlayOneShot(AudioClip _clipToPlay)
    {
        myAudioSource.PlayOneShot(_clipToPlay);
    }

    public void PlayWinAudioClip()
    {
        myAudioSource.PlayOneShot(WinAudioClip);
    }
}
