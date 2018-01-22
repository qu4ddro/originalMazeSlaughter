using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Items
{
    public float TorchMultiplier = 5.0f;
    public float TorchDuration = 5.0f;
    public float FadeDuration = 0.5f;

    private GameObject Mask;

    //private GameObject Light;

    // Use this for initialization
    void OnEnable()
    {
        Mask = GameObject.FindWithTag("Mask");

        //        Light = GameObject.FindWithTag("Light");

        float newMaskSize = Mask.transform.localScale.x * TorchMultiplier;

        //float newLightSize = Light.GetComponent<Light>().range * TorchMultiplier;

        myAudioSource.PlayOneShot(UseItemAudioClip);

        StartCoroutine("FadeToNewLightSize",newMaskSize);

        //StartCoroutine("FadeToNewLightSize",newLightSize);
        StartCoroutine("DeactivateAfterDuration");
    }

    IEnumerator FadeToNewLightSize(float _newLightSize)
    {
        float currentSize = Mask.transform.localScale.x;
        //float currentSize = Light.GetComponent<Light>().range;
        float remainingDistance = _newLightSize - currentSize;
        bool rising = remainingDistance > 0;

        float elapsedTime = 0;
        
        while (Math.Abs(remainingDistance) > float.Epsilon)
        {
            float newSize = Mathf.Lerp(currentSize, _newLightSize, elapsedTime / FadeDuration);
            Mask.transform.localScale = new Vector3(newSize, newSize, newSize);
            
            //Light.GetComponent<Light>().range = Mathf.Lerp(currentSize,_newLightSize,elapsedTime/FadeDuration);
            currentSize = Mask.transform.localScale.x;
            
            //currentSize = Light.GetComponent<Light>().range;
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

        float newLightSize = Mask.transform.localScale.x / TorchMultiplier;
        //float newLightSize = Light.GetComponent<Light>().range / TorchMultiplier;
        
        StopCoroutine("FadeToNewLightSize");
        StartCoroutine("FadeToNewLightSize",newLightSize);
    }
}
