using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * HeatableContainers behave like any normal Heatable object, but also heat the objects they contain.
 */

public class HeatableContainer : Heatable
{
    [SerializeField] private Heater heater; // Heater

    private void Update()
    {
        heater.currentTemp = currentTemp; // Set the temperature of the container's heater to the container's current temperature.
    }

}
