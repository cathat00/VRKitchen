using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainer : RecipeBroadcaster
{
    public GameObject liquid;
    public GameObject liquidMesh;
    private MeshRenderer liquidRender;

    [SerializeField] private int sloshSpeed;
    [SerializeField] private int rotateSpeed;

    private float curRotationDiff;
    [SerializeField] private float maxRotationDiff;
    [SerializeField] private float minRotationDiff;

    [SerializeField] private float maxVolume = 0f; // In liters
    [SerializeField] private float currentVolume = 0f; // In liters

    [SerializeField] private Transform fillPoint;
    private Vector3 startingHeight;

    private void Awake()
    {
        startingHeight = liquid.transform.localPosition;
        liquidRender = liquidMesh.GetComponent<MeshRenderer>();

        if (currentVolume <= 0f) liquidRender.enabled = false; // Only show liquid in the container if there is a nonzero volume of said liquid
    }

    // Update is called once per frame
    void Update()
    {
        Slosh();
        
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
        Vector3 heightByVolume = Vector3.Lerp(startingHeight, fillPoint.localPosition, currentVolume / maxVolume);
        liquid.transform.localPosition = heightByVolume;

        // The liquid should have the highest rotation difference in the middle of the container
        float pingPong = Mathf.PingPong(currentVolume / (maxVolume / 2), 1);
        curRotationDiff = Mathf.Lerp(minRotationDiff, maxRotationDiff, pingPong);
    }

    public void AddToVolume(float amt)
    {
        currentVolume = Mathf.Clamp(currentVolume + amt, 0, maxVolume);

        if (liquidRender.enabled == false) liquidRender.enabled = true;

        AdjustLiquidHeight();

        Ingredient liquidIng = liquid.GetComponent<Ingredient>();
        BroadcastSet(liquidIng.type, currentVolume);
    }

    public float ClampRotationValue(float value, float difference)
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
