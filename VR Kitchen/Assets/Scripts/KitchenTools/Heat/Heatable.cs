using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Heatable : MonoBehaviour
{
    public float maxTemp;
    public float currentTemp = 0.0f;
    [SerializeField] private float targetTemp = 0f;

    [SerializeField] private float degsPerSec; // Speed at which the eye changes from one heat setting to the next in degrees per second

    private bool beingHeated = false;
    [SerializeField] protected bool canCool = true;

    private IEnumerator controlTemp = null;

    protected virtual void OnEnable()
    {
        controlTemp = ControlTemp();
        StartCoroutine(controlTemp);
    }

    public void Heat(float temp) => targetTemp = temp;

    public void RemoveFromHeat() => targetTemp = 0f;

    private IEnumerator ControlTemp()
    {

        float totalTime;

        float startingTemp = 0.0f;
        bool cooling = false;

        float timeElapsed = 0.0f;

        while (this.gameObject.activeInHierarchy)
        {
            if (targetTemp < currentTemp && !cooling && canCool)
            {
                startingTemp = currentTemp;
                cooling = true;
                timeElapsed = 0.0f;
            }
            else if (targetTemp > currentTemp && cooling)
            {
                startingTemp = 0.0f;
                cooling = false;
                timeElapsed = 0.0f;
            }

            totalTime = Mathf.Abs(startingTemp - targetTemp) / degsPerSec; // Recalculate the total time on each loop -- it is liable to change as the heater's temp changes

            if (totalTime > timeElapsed)
            {
                timeElapsed += Time.deltaTime;
                currentTemp = Mathf.Lerp(startingTemp, targetTemp, timeElapsed / totalTime);
            }

            yield return null;
        }
    } // End of control temp

}
