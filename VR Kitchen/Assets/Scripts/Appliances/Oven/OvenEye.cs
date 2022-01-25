using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenEye : MonoBehaviour
{
    private Material mat;
    
    [SerializeField] private Color offShade;
    [SerializeField] private Color maxShade;

    [SerializeField] private float maxTemp;
    private float tempIncrement = 100;
    private float currentTemp = 0.0f;

    [SerializeField] private float tempChangeSpeed; // Speed at which the eye changes from one heat setting to the next

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
            StopAllCoroutines();
            StartCoroutine(changeTemp(linkedDial.currentDialValue * tempIncrement));
        }
        ApplyHeatToObject();
    }

    IEnumerator changeTemp(float targetTemp)
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
            updateEyeShade(currentTemp);
            yield return null;
        }
    }

    private void ApplyHeatToObject() // Heat objects on the eye
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.up);
        Physics.Raycast(ray, out hit, 1.0f);

        GameObject objToHeat = hit.collider.gameObject;
        if (objToHeat != null && objToHeat.tag == "Heatable")
        {
            objToHeat.GetComponent<Heatable>().ChangeTemp(currentTemp);
        }
    }

    private void updateEyeShade(float temp) => mat.color = Color.Lerp(offShade, maxShade, temp / maxTemp);

}
