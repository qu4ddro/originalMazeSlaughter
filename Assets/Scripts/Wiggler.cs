using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour
{
    public Light light;
    public float speed;
    public float amount;

    private float lastWiggle;
    private float originalLightSize;

    private void Start()
    {
        originalLightSize = light.intensity;
    }

    // Update is called once per frame
    void Update () {
	    if (speed > 0 && amount > 0)
	    {
	        lastWiggle += Time.deltaTime;

	        if (lastWiggle > 1f / speed)
	        {
	            lastWiggle = 0;
	            float newLightSize = originalLightSize*Random.Range(1-amount, 1+amount);
	            StopCoroutine("FadeToNewLightSize");
                StartCoroutine("FadeToNewLightSize", newLightSize);
            }
        }
    }

    IEnumerator FadeToNewLightSize(float _newLightSize)
    {
        float currentSize = light.intensity;
        float remainingDistance = _newLightSize - currentSize;
        float elapsedTime = 0;

        while (Mathf.Abs(remainingDistance) > float.Epsilon)
        {
            light.intensity = Mathf.Lerp(currentSize, _newLightSize,  elapsedTime / (1f/speed));

            currentSize = light.intensity;
            remainingDistance = _newLightSize - currentSize;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
