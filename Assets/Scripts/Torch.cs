using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Items
{
    public float TorchMultiplier = 5.0f;
    public float TorchDuration = 5.0f;
    public float FadeDuration = 0.5f;


    private GameObject Light;

    // Use this for initialization
    void OnEnable()
    {
        Light = GameObject.FindWithTag("Light");

        float newLightSize = Light.GetComponent<Light>().range * TorchMultiplier;

        myAudioSource.PlayOneShot(UseItemAudioClip);

        StartCoroutine("FadeToNewLightSize",newLightSize);
        StartCoroutine("DeactivateAfterDuration");
    }

    IEnumerator FadeToNewLightSize(float _newLightSize)
    {
        float currentSize = Light.GetComponent<Light>().range;
        float remainingDistance = _newLightSize - currentSize;
        bool rising = remainingDistance > 0;

        float elapsedTime = 0;
        
        while (Math.Abs(remainingDistance) > float.Epsilon)
        {
            float newSize = Mathf.Lerp(currentSize, _newLightSize, elapsedTime / FadeDuration);
            
            Light.GetComponent<Light>().range = Mathf.Lerp(currentSize,_newLightSize,elapsedTime/FadeDuration);
            
            currentSize = Light.GetComponent<Light>().range;
            remainingDistance = _newLightSize - currentSize;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (!rising)
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator DeactivateAfterDuration()
    {
        // Wait for an amount of Time before resetting
        yield return new WaitForSeconds(TorchDuration);

        float newLightSize = Light.GetComponent<Light>().range / TorchMultiplier;
        
        StopCoroutine("FadeToNewLightSize");
        StartCoroutine("FadeToNewLightSize",newLightSize);
    }
}
