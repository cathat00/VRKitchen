using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour
{
    private List<GameObject> objsHeating = new List<GameObject>();
    public float currentTemp;

    private void Update()
    {
        foreach (GameObject obj in objsHeating)
        {
            obj.GetComponent<Heatable>().Heat(currentTemp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        Heatable heatableObj;
        if (obj != null && obj.tag == "Heatable")
        {
            heatableObj = obj.GetComponent<Heatable>();
            heatableObj.Heat(currentTemp); // Apply the temperature of the heater to the heatable object
            objsHeating.Add(obj); // Add this object to the list of objects this eye is heating
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        if (objsHeating.Contains(obj))
        {
            obj.GetComponent<Heatable>().RemoveFromHeat(); // Stop heating the object if it is no longer on the eye
            objsHeating.Remove(obj);
        }
    }
}
