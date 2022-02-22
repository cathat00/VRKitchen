using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour
{
    // Start is called before the first frame update

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

    //public void HeatObjects(float currentTemp) // Heat objects on the heater
    //{
    //    GameObject objHit;
    //    Heatable heatableObj;
    //    List<GameObject> newHeatingList = new List<GameObject>();

    //    Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(transform.localScale.x * 2, .05f, transform.localScale.z * 2), Quaternion.identity);
      
    //    foreach (Collider col in hitColliders)
    //    {
    //        objHit = col.gameObject;

    //        if (objHit != null && objHit.tag == "Heatable")
    //        {
    //            heatableObj = objHit.GetComponent<Heatable>();
    //            heatableObj.Heat(currentTemp); // Apply the temperature of the heater to the heatable object
    //            newHeatingList.Add(objHit); // Add this object to the list of objects this eye is heating
    //        }
    //    } // End of foreach

    //    foreach (GameObject obj in newHeatingList) // Check for objects that have been taken off of the stove eye
    //    {
    //        if (!newHeatingList.Contains(obj))
    //        {
    //            Debug.Log("remove obj from heat");
    //            obj.GetComponent<Heatable>().RemoveFromHeat(); // Stop heating the object if it is no longer on the eye
    //            objsHeating.Remove(obj);
    //        }
    //    }
    //}

    //public void HeatObjectsRay(float currentTemp) // Heat objects on the heater
    //{
    //    RaycastHit[] allHits;
    //    Ray ray = new Ray(transform.position, Vector3.up);
    //    allHits = Physics.RaycastAll(ray, .05f);

    //    GameObject objHit;
    //    Heatable heatableObj;
    //    List<GameObject> newHeatingList = new List<GameObject>();

    //    foreach (RaycastHit hit in allHits)
    //    {
    //        objHit = hit.collider.gameObject;

    //        if (objHit != null && objHit.tag == "Heatable")
    //        {
    //            heatableObj = objHit.GetComponent<Heatable>();
    //            heatableObj.Heat(currentTemp); // Apply the temperature of the heater to the heatable object
    //            newHeatingList.Add(objHit); // Add this object to the list of objects this eye is heating
    //        }
    //    } // End of foreach

    //    foreach (GameObject obj in objsHeating.GetRange(0, objsHeating.Count)) // Check for objects that have been taken off of the stove eye
    //    {
    //        if (!newHeatingList.Contains(obj))
    //        {
    //            obj.GetComponent<Heatable>().RemoveFromHeat(); // Stop heating the object if it is no longer on the eye
    //            objsHeating.Remove(obj);
    //        }
    //    }
    //    objsHeating = newHeatingList;
    //}

}
