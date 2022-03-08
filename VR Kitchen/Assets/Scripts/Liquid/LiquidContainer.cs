using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    public GameObject liquid;
    public GameObject liquidMesh;

    [SerializeField] private int sloshSpeed;
    [SerializeField] private int rotateSpeed;

    private float curRotationDiff;
    [SerializeField] private float maxRotationDiff;
    [SerializeField] private float minRotationDiff;

    [SerializeField] private float maxVolume = 0f; // In liters
    private float currentVolume = 0f; // In liters

    [SerializeField] private Transform fillPoint;
    private Vector3 startingHeight;

    private void Awake()
    {
        startingHeight = liquid.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Slosh();
        Fill();
        
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

    void Fill()
    {
        Vector3 heightByVolume = Vector3.Lerp(startingHeight, fillPoint.localPosition, currentVolume / maxVolume);
        if (liquid.transform.localPosition != heightByVolume)
        {
            liquid.transform.localPosition = heightByVolume;
        }

        // The liquid should have the highest rotation difference in the middle of the container
        float pingPong = Mathf.PingPong(currentVolume / (maxVolume / 2), 1);
        curRotationDiff = Mathf.Lerp(minRotationDiff, maxRotationDiff, pingPong);
    }

    public void AddToVolume(float amt)
    {
        currentVolume = Mathf.Clamp(currentVolume + amt, 0, maxVolume);
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
