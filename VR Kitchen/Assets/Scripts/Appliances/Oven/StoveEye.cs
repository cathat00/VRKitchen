using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveEye : MonoBehaviour
{
    private Material mat;

    [SerializeField] private Color offShade;
    [SerializeField] private Color maxShade;

    public float maxTemp;
    private float currentTemp = 0.0f;
    public float tempChangeSpeed; // Speed at which the eye changes from one heat setting to the next

    [SerializeField] private OvenDial linkedDial;
    private int dialValue = 0;
    private int tempIncrement = 100;

    private List<GameObject> objsHeating = new List<GameObject>();

    private IEnumerator adjustTemp = null;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialValue != linkedDial.currentDialValue) // Dial value was changed
        {
            ChangeTemp(linkedDial.currentDialValue * tempIncrement);
        }
        UpdateEyeShade();
        HeatObjects();

    }
    private void HeatObjects() // Heat objects on the eye
    {
        RaycastHit[] allHits;
        Ray ray = new Ray(transform.position, Vector3.up);
        allHits = Physics.RaycastAll(ray, .05f);

        GameObject objHit;
        Heatable heatableObj;
        List<GameObject> newHeatingList = new List<GameObject>();

        foreach (RaycastHit hit in allHits)
        {
            objHit = hit.collider.gameObject;

            if (objHit != null && objHit.tag == "Heatable")
            {
                heatableObj = objHit.GetComponent<Heatable>();
                heatableObj.Heat(currentTemp); // Apply the temperature of the heater to the heatable object
                newHeatingList.Add(objHit); // Add this object to the list of objects this eye is heating

                // If the object is not the temp of the eye AND the object is not currently changing its temp to be so, then start changing the objs temp
                //if ( (!heatableObj.isChangingTemp) && (heatableObj.currentTemp != currentTemp) )
                //{
                //    StartCoroutine(TransferHeat(heatableObj));
                //}

            }
        } // End of foreach

        foreach (GameObject obj in objsHeating.GetRange(0, objsHeating.Count)) // Check for objects that have been taken off of the stove eye
        {
            if (!newHeatingList.Contains(obj))
            {
                obj.GetComponent<Heatable>().RemoveFromHeat(); // Stop heating the object if it is no longer on the eye
                objsHeating.Remove(obj);
            }
        }
        objsHeating = newHeatingList;

    }

    private void UpdateEyeShade() => mat.color = Color.Lerp(offShade, maxShade, currentTemp / maxTemp); // Change color of the stove eye

    public virtual void ChangeTemp(float targetTemp)
    {
        if (adjustTemp != null) StopCoroutine(adjustTemp);

        adjustTemp = AdjustTempOverTime(targetTemp);
        StartCoroutine(adjustTemp);
    }

    private IEnumerator AdjustTempOverTime(float targetTemp)
    {
        float startingTemp = currentTemp;
        if (targetTemp > maxTemp)
        {
            targetTemp = maxTemp;
        }

        float totalTime = Mathf.Abs(targetTemp - startingTemp) / tempChangeSpeed; // I.e. it takes twice as long to heat from 100 -> 300 as from 100 -> 200
        float timeElapsed = 0.0f;

        dialValue = linkedDial.currentDialValue;

        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            currentTemp = Mathf.Lerp(startingTemp, targetTemp, timeElapsed / totalTime);
            yield return null;
        }
    }

    //private IEnumerator TransferHeat(Heatable obj)
    //{
    //    float startingTemp = obj.maxTemp;

    //    float totalTime = Mathf.Abs(currentTemp - startingTemp) / obj.tempChangeSpeed; // I.e. it takes twice as long to heat from 100 -> 300 as from 100 -> 200
    //    float timeElapsed = 0.0f;

    //    obj.isChangingTemp = true;
    //    while (timeElapsed < totalTime)
    //    {
    //        totalTime = Mathf.Abs(currentTemp - startingTemp) / obj.tempChangeSpeed; // Recalculate the total time on each loop -- the heat of the eye is liable to change during the transfer
    //        timeElapsed += Time.deltaTime;

    //        obj.currentTemp = Mathf.Lerp(startingTemp, currentTemp, timeElapsed / totalTime);
    //        yield return null;
    //    }
    //    obj.isChangingTemp = false;
    //}

}
