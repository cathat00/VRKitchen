using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Stove Eyes are controlled by linked oven dials (SEE OvenDial class). Each setting on the dial represents a certain temperature for the stove eye (e.g. 1 = 100F, 2 = 200F, etc.)
 * The stove eye heats and cools towards a target temperature (determined by the dial setting) as a function of time.
 */

public class StoveEye : MonoBehaviour
{
    private Material mat;

    [SerializeField] private Color offShade; // Color of the stove eye when it is completely off
    [SerializeField] private Color maxShade; // Color of the stove eye at maximum heat

    public float maxTemp; // Maximum temperature that the stove eye can reach
    [SerializeField] private float currentTemp = 0.0f;
    public float degsPerSec; // Rate at which the eye changes temperature

    [SerializeField] private OvenDial linkedDial; // Oven dial that controls the heat of the eye
    private int dialValue = 0;
    private int tempIncrement = 100; // Dial setting 1 -> target temp = 100 degs, 2 -> 200 degs, etc. 

    [SerializeField] Heater heater;

    private IEnumerator adjustTemp = null;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialValue != linkedDial.currentDialValue) // Dial value was changed!
        {
            ChangeTemp(linkedDial.currentDialValue * tempIncrement); // Move toward new target temperature
        }
        UpdateEyeShade();
        heater.currentTemp = currentTemp; // Update the heat of the eye's heating plane
    }

    private void UpdateEyeShade() => mat.color = Color.Lerp(offShade, maxShade, currentTemp / maxTemp); // Change color of the stove eye

    public virtual void ChangeTemp(float targetTemp)
    {
        if (adjustTemp != null) StopCoroutine(adjustTemp);

        adjustTemp = AdjustTempOverTime(targetTemp);
        StartCoroutine(adjustTemp);
    }

    private IEnumerator AdjustTempOverTime(float targetTemp) // Adjust the temperature of the stove eye as a function of time
    {
        float startingTemp = currentTemp;
        if (targetTemp > maxTemp)
        {
            targetTemp = maxTemp;
        }

        float totalTime = Mathf.Abs(targetTemp - startingTemp) / degsPerSec; // I.e. it takes twice as long to heat from 100 -> 300 as from 100 -> 200
        float timeElapsed = 0.0f; // Timer

        dialValue = linkedDial.currentDialValue;

        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime; // Add framerate to timer
            currentTemp = Mathf.Lerp(startingTemp, targetTemp, timeElapsed / totalTime); // Heat / Cool
            yield return null;
        }
    }

}
