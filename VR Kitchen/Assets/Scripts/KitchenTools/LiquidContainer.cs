using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script governs any object that can be filled with liquid, this may include bowls, cups, pots, pans, etc. Spices can be added to the container, as well
 * as liquid. The script reports to the recipe manager with its current contents, including the amount of its individual contents in given units. The script
 * also reports the current heat of its liquid to the recipe system. 
 * 
 * In addition to reporting to the recipe system, this script manipulates the attached liquid object in the following ways:
 * - liquid object rotates inversely to this liquid object. This is handled by the Slosh() function of this class.
 * - liquid object can scale and rise as new liquid is added to the container, this is governed by the AdjustLiquidHeight function of this class
 * - liquid object changes color as liquids of different color are added to the volume of the container
 */

public class LiquidContainer : RecipeBroadcaster
{
    public GameObject liquid;
    public GameObject liquidMesh;
    private Heatable liquidHeat = null; // The heatable script of the liquid object
    [HideInInspector] public MeshRenderer liqRenderer;

    [SerializeField] protected float maxVolume = 0f; // In liters
    [SerializeField] public float totalVolume = 0f; // In liters
    private Dictionary<string, float> volumeMap = new Dictionary<string, float>(); // Volumes of individual liquid types, in liters
    private Dictionary<string, float> spiceMap = new Dictionary<string, float>(); // Amount of individual spice types, in grams

    [SerializeField] private int sloshSpeed;
    [SerializeField] private int rotateSpeed;

    private float curRotationDiff; // Difference between the worldspace UP vector and the liquid's UP vector (used to create "slosh" effect)
    [SerializeField] private float maxRotationDiff;
    [SerializeField] private float minRotationDiff;

    [SerializeField] private Vector3 endScale;
    private Vector3 startScale;

    private string currentLiquidType = ""; // Current liquid type being added. Variable used for color blending
    private string lastLiquidType = ""; // Last liquid type added. Variable used for color blending
    private Color lastColor; // "Starting" color for blend from color -> color

    [SerializeField] private Transform fillPoint;
    private Vector3 startingHeight;

    private void Awake()
    {
        startingHeight = liquid.transform.localPosition;
        startScale = liquid.transform.localScale;

        liqRenderer = liquidMesh.GetComponent<MeshRenderer>();

        liquid.TryGetComponent<Heatable>(out liquidHeat);

        if (totalVolume <= 0f) liqRenderer.enabled = false; // Only show liquid in the container if there is a nonzero volume of said liquid
    }

    // Update is called once per frame
    void Update()
    {
        Slosh();

        // If the liquid is heatable, broadcast its heat to the recipe system
        if (liquidHeat != null) BroadcastSet("Heat", liquidHeat.currentTemp);

        liquidMesh.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self); // Spin
    }

    void Slosh()
    {
        Quaternion inverseRotation = Quaternion.Inverse(transform.localRotation); // Inverse of the container's rotation
        Vector3 finalRot = Quaternion.RotateTowards(liquid.transform.localRotation, inverseRotation, sloshSpeed * Time.deltaTime).eulerAngles;

        finalRot.x = ClampRotationValue(finalRot.x, curRotationDiff);
        finalRot.z = ClampRotationValue(finalRot.z, curRotationDiff);

        liquid.transform.localEulerAngles = finalRot;
    }

    void AdjustLiquidHeight()
    {
        Vector3 heightByVolume = Vector3.Lerp(startingHeight, fillPoint.localPosition, totalVolume / maxVolume);
        liquid.transform.localPosition = heightByVolume;

        // The liquid should have the highest rotation difference in the middle of the container
        float pingPong = Mathf.PingPong(totalVolume / (maxVolume / 2), 1);
        curRotationDiff = Mathf.Lerp(minRotationDiff, maxRotationDiff, pingPong);

        // The liquid can scale as its volume increases
        liquid.transform.localScale = Vector3.Lerp(startScale, endScale, totalVolume / maxVolume);
    }

    public void AddLiquid(float amt, string liquidType, Color liquidColor)
    {
        if (liqRenderer.enabled == false) liqRenderer.enabled = true; // Display liquid mesh

        totalVolume = Mathf.Clamp(totalVolume + amt, 0, maxVolume);
        if ( !(totalVolume >= maxVolume) ) AdjustLiquidHeight(); // Do not adjust height if at max volume

        AddToVolumeMap(liquidType, amt);
        BroadcastSet(liquidType, volumeMap[liquidType]); // Broadcast to the recipe system

        DetermineLiquidHue(liquidType, liquidColor);
    }

    public void AddSpice(float amt, string spiceType) // Spices do not contribute to total volume
    {
        AddToSpiceMap(spiceType, amt);
        BroadcastSet(spiceType, spiceMap[spiceType]); // Broadcast to the recipe system
    }

    private void AddToVolumeMap(string liquidType, float amt)
    {
        if (volumeMap.ContainsKey(liquidType))
        {
            volumeMap[liquidType] = Mathf.Clamp(volumeMap[liquidType] + amt, 0, totalVolume); // Volume of liquid cannot exceed the total volume of all the liquids
        }
        else volumeMap[liquidType] = amt;

        if (totalVolume >= maxVolume) // Liquid displaces other liquids through overflow if volume maxed out
        {
            string[] temp = new string[volumeMap.Keys.Count];
            volumeMap.Keys.CopyTo(temp, 0);
            foreach (string l in temp)
            {
                volumeMap[l] = Mathf.Clamp(volumeMap[l] - amt, 0, maxVolume); // Cannot go below zero
            }
        }
    }

    private void AddToSpiceMap(string spiceType, float amt)
    {
        if (spiceMap.ContainsKey(spiceType)) spiceMap[spiceType] += amt;
        else spiceMap[spiceType] = amt;
    }

    private void DetermineLiquidHue(string liquidType, Color liquidColor) // Determine the color of the liquid by type, blend liquid colors
    {
        if (currentLiquidType == "")
        {
            currentLiquidType = liquidType; lastLiquidType = liquidType; // First liquid added
        }
        else if (currentLiquidType != liquidType)
        {
            lastLiquidType = currentLiquidType;
            currentLiquidType = liquidType;
            lastColor = liqRenderer.material.color;
        }

        Color newColor = Color.Lerp(lastColor, liquidColor, volumeMap[liquidType] / totalVolume);
        liqRenderer.material.color = newColor;
    }

    public float ClampRotationValue(float value, float difference) // "Slosh" rotation of the liquid should not exceed a given amount
    {
        float returnValue = 0.0f;

        if (value > 180)
        {
            returnValue = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            returnValue = Mathf.Clamp(value, 0, difference);
        }
        return returnValue;
    }
}
