using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSlosh : MonoBehaviour
{
    public GameObject liquid;
    public GameObject liquidMesh;

    [SerializeField] private int sloshSpeed = 60;
    [SerializeField] private int rotateSpeed = 15;

    [SerializeField] private int rotDifference = 25;

    // Update is called once per frame
    void Update()
    {
        SloshRotate();
        //SloshMove();

        liquidMesh.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self); // Spin
    }

    void SloshRotate()
    {
        Quaternion inverseRotation = Quaternion.Inverse(transform.localRotation); // Inverse of the container's rotation
        Vector3 finalRot = Quaternion.RotateTowards(liquid.transform.localRotation, inverseRotation, sloshSpeed * Time.deltaTime).eulerAngles;

        finalRot.x = ClampRotationValue(finalRot.x, rotDifference);
        finalRot.z = ClampRotationValue(finalRot.z, rotDifference);

        liquid.transform.localEulerAngles = finalRot;
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
