using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour
{
    public Light light;
    public float speed;
    public float amount;

    private float lastWiggle;

    // Update is called once per frame
    void Update () {
	    if (speed > 0 && amount > 0)
	    {
	        lastWiggle += Time.deltaTime;

	        if (lastWiggle > 1f / speed)
	        {       // flashFrequency is int
	            lastWiggle = 0;
	            float newLightSize = Random.Range(light.range - amount, light.range + amount);
	            StopCoroutine("FadeToNewLightSize");
                StartCoroutine("FadeToNewLightSize", newLightSize);
            }
        }
    }

    IEnumerator FadeToNewLightSize(float _newLightSize)
    {
        float currentSize = light.range;
        float remainingDistance = _newLightSize - currentSize;
        float elapsedTime = 0;

        while (Mathf.Abs(remainingDistance) > float.Epsilon)
        {
            float newSize = Mathf.Lerp(currentSize, _newLightSize, elapsedTime / 0.2f);

            light.range = Mathf.Lerp(currentSize, _newLightSize, elapsedTime / 0.2f);

            currentSize = light.range;
            remainingDistance = _newLightSize - currentSize;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
