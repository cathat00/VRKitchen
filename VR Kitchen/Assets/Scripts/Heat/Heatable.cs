using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the base class for heatable objects. Any object that can receive heat from another is considered "Heatable" -- this includes pots and pans, liquid and solid ingredients, etc.
 * A heatable object heats or cools towards a given target temperature as a function of time.
 */

public abstract class Heatable : MonoBehaviour
{
    [SerializeField] protected float maxTemp; // Maximum temperature the object can reach
    [SerializeField] public float currentTemp = 0.0f; // Current temperature of the object
    protected float targetTemp = 0f; // Temperatue that the object is approaching (can be less than or greater than current temperature)

    [SerializeField] protected float degsPerSec; // Rate at which object changes temperature

    [SerializeField] protected bool canCool = true; // Determines whether or not the object can cool

    private IEnumerator controlTemp = null; // Coroutine to handle heating/cooling

    protected virtual void OnEnable()
    {
        controlTemp = ControlTemp();
        StartCoroutine(controlTemp);
    }

    public void Heat(float temp) => targetTemp = Mathf.Clamp(temp, 0, maxTemp); // Clamp target temperature value between 0 degs and maxTemp

    public void RemoveFromHeat() => targetTemp = 0f; // Begin cooling down to 0 degrees

    protected virtual IEnumerator ControlTemp()
    {

        float totalTime; // Total time it will take to reach target temperature from current temperature

        float startingTemp = 0.0f;
        bool cooling = false;

        float timeElapsed = 0.0f; // Time elapsed in heating / cooling process

        while (this.gameObject.activeInHierarchy) // While object is active...
        {
            if (targetTemp < currentTemp && !cooling && canCool) // Quit heating and begin cooling
            {
                startingTemp = currentTemp;
                cooling = true; // Object is no longer heating, now cooling
                timeElapsed = 0.0f; // Reset time
            }
            else if (targetTemp > currentTemp && cooling) // Begin heating
            {
                startingTemp = 0.0f;
                cooling = false;
                timeElapsed = 0.0f; // Reset time
            }

            totalTime = Mathf.Abs(startingTemp - targetTemp) / degsPerSec; // Recalculate the total time on each loop -- it is liable to change as the heater's temp changes

            if (totalTime > timeElapsed)
            {
                timeElapsed += Time.deltaTime; // Add framerate to timer
                currentTemp = Mathf.Lerp(startingTemp, targetTemp, timeElapsed / totalTime); // Change temperature as a function of time
            }

            yield return null;
        }
    } // End of control temp

}
