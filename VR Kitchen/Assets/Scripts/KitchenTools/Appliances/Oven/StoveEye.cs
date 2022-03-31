using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveEye : MonoBehaviour
{
    private Material mat;

    [SerializeField] private Color offShade;
    [SerializeField] private Color maxShade;

    public float maxTemp;
    [SerializeField] private float currentTemp = 0.0f;
    public float degsPerSec; // Rate at which the eye changes temperature

    [SerializeField] private OvenDial linkedDial;
    private int dialValue = 0;
    private int tempIncrement = 100;

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
        if (dialValue != linkedDial.currentDialValue) // Dial value was changed
        {
            ChangeTemp(linkedDial.currentDialValue * tempIncrement);
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

    private IEnumerator AdjustTempOverTime(float targetTemp)
    {
        float startingTemp = currentTemp;
        if (targetTemp > maxTemp)
        {
            targetTemp = maxTemp;
        }

        float totalTime = Mathf.Abs(targetTemp - startingTemp) / degsPerSec; // I.e. it takes twice as long to heat from 100 -> 300 as from 100 -> 200
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
