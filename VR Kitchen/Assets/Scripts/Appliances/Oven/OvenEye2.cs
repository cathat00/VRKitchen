using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenEye2 : Heatable
{
    private Material mat;

    [SerializeField] private Color offShade;
    [SerializeField] private Color maxShade;

    private int dialValue = 0;

    [SerializeField] private OvenDial linkedDial;

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
        ApplyHeatToObject();

    }
    private void ApplyHeatToObject() // Heat objects on the eye
    {
        RaycastHit[] allHits;
        Ray ray = new Ray(transform.position, Vector3.up);
        allHits = Physics.RaycastAll(ray, .05f);

        GameObject objHit;
        foreach (RaycastHit hit in allHits)
        {
            objHit = hit.collider.gameObject;
            
            if (objHit != null && objHit.tag == "Heatable")
            {
                objHit.GetComponent<Heatable>().ChangeTemp(currentTemp);
            }
        }
  
    }

    private void UpdateEyeShade() => mat.color = Color.Lerp(offShade, maxShade, currentTemp / maxTemp); // Change color of the stove eye

    protected override IEnumerator AdjustTempOverTime(float targetTemp)
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

}
