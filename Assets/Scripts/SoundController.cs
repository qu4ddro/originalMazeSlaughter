using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip AmbientAudioClip;
    public AudioClip StartAudioClip;
    public AudioClip WinAudioClip;
    public AudioClip KillerStartAudioClip;

    public float StartVolumeAmbientSound = 0.65f;

    private AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = AmbientAudioClip;
        audioSource.volume = 0;
        audioSource.Play();
        StartCoroutine(FadeSound(StartVolumeAmbientSound, 3f));

        Invoke("PlayDelayedKillerStartClip", 2f);

        //Invoke("PlayDelayerStartClip", 2f);
    }

    public IEnumerator FadeSound(float _newVolume, float FadeDuration)
    {
        float currentVolume = audioSource.volume;
        float startTime = Time.time;
        float elapsedTime = 0;
        float remaining = _newVolume - currentVolume;

        while (Mathf.Abs(remaining) > float.Epsilon)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, _newVolume, elapsedTime / FadeDuration);
            elapsedTime = Time.time - startTime;
            remaining = _newVolume - audioSource.volume;
            yield return null;
        }
    }

    public void PlayDelayedStartClip()
    {
        audioSource.PlayOneShot(StartAudioClip);
    }

    public void PlayDelayedKillerStartClip()
    {
        audioSource.PlayOneShot(KillerStartAudioClip);
    }

    public void PlayOneShot(AudioClip _clipToPlay)
    {
        audioSource.PlayOneShot(_clipToPlay);
    }

    public void PlayWinAudioClip()
    {
        audioSource.PlayOneShot(WinAudioClip);
    }
}
