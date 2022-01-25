using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Heatable : MonoBehaviour
{
    public float maxTemp;
    protected float tempIncrement = 100;
    protected float currentTemp = 0.0f;

    public float tempChangeSpeed; // Speed at which the eye changes from one heat setting to the next

    public virtual void ChangeTemp(float targetTemp)
    {
        StopAllCoroutines();
        StartCoroutine(AdjustTempOverTime(targetTemp));
    }

    protected virtual IEnumerator AdjustTempOverTime(float targetTemp) // Lerp between specified temperatures, updating the obj accordingly
    {
        float startingTemp = currentTemp;
        if (targetTemp > maxTemp)
        {
            targetTemp = maxTemp;
        }

        float totalTime = Mathf.Abs(targetTemp - startingTemp) / tempChangeSpeed; // I.e. it takes twice as long to heat from 100 -> 300 as from 100 -> 200
        float timeElapsed = 0.0f;

        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            currentTemp = Mathf.Lerp(startingTemp, targetTemp, timeElapsed / totalTime);
            yield return null;
        }
    }
}
