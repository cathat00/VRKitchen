using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class defines the behavior of a heatable liquid. A heatable liquid behaves like any normal Heatable object, but boils once it reaches a certain temperature.
 * Importantly, heatable liquids ARE NOT heatable while their mesh is disabled. This is done to prevent the heating of liquid in a container of volume 0. 
 */

public class HeatableLiquid : Heatable
{
    [SerializeField] Renderer mesh;

    [SerializeField] GameObject particles; // Particles to display once the liquid begins boiling
    [SerializeField] float boilingPoint; // Temperature at which the liquid will boil

    void Update()
    {
        // Display particles once the liquid boils, but only if the liquid's mesh renderer is active
        particles.SetActive( (currentTemp >= boilingPoint) && mesh.enabled);
    }

    protected override IEnumerator ControlTemp()
    {
        float totalTime;

        float startingTemp = 0.0f;
        bool cooling = false;

        float timeElapsed = 0.0f;

        while (this.gameObject.activeInHierarchy)
        {
            if (mesh.enabled) // ONLY CHANGE FROM BASE CLASS -> liquid cannot heat while mesh is not enabled
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
            }

            yield return null;
        }
    }
}
