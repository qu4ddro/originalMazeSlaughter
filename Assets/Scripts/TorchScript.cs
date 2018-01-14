using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

public class TorchScript : MonoBehaviour
{

    public float TorchMultiplier = 5.0f;
    public float TorchDuration = 5.0f;
    public float FadeDuration = 1.0f;

    private GameObject Mask;


    // Use this for initialization
    void OnEnable()
    {
        Mask = GameObject.FindWithTag("Mask");

        Vector3 newMaskSize = Mask.GetComponent<Transform>().localScale * TorchMultiplier;
        StartCoroutine("FadeToNewMaskSize",newMaskSize);
        StartCoroutine("DeactivateAfterDuration");
    }

    IEnumerator FadeToNewMaskSize(Vector3 _newMaskSize)
    {
        Vector3 currentSize = Mask.GetComponent<Transform>().localScale;
        float remainingDistance = _newMaskSize.magnitude - currentSize.magnitude;
        Debug.Log("FadeStart");
        bool rising = remainingDistance > 0;

        float elapsedTime = 0;
        
        while (Math.Abs(remainingDistance) > float.Epsilon)
        {
            Mask.GetComponent<Transform>().localScale = Vector3.Lerp(currentSize,_newMaskSize,elapsedTime/FadeDuration);
            currentSize = Mask.GetComponent<Transform>().localScale;
            remainingDistance = _newMaskSize.magnitude - currentSize.magnitude;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("FadeStop");
        if (!rising)
        {
            this.gameObject.SetActive(false);
            Debug.Log("Ich schalt mich ab");
        }
    }

    IEnumerator DeactivateAfterDuration()
    {
        // Wait for an amount of Time before resetting
        yield return new WaitForSeconds(TorchDuration);

        Debug.Log("Licht geht wieder aus");

        Vector3 newMaskSize = Mask.GetComponent<Transform>().localScale / TorchMultiplier;
        
        StopCoroutine("FadeToNewMaskSize");
        StartCoroutine("FadeToNewMaskSize",newMaskSize);

        
    }
}
