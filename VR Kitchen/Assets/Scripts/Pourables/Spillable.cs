using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spillable : MonoBehaviour
{
    [SerializeField] private float spillThreshold = 45f;
    private PourDetector pourDetector;

    private void Start()
    {
        pourDetector = GetComponent<PourDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CalculateSpillAngle());
        if (CalculateSpillAngle() > spillThreshold)
        {
            pourDetector.isPouring = true;
        }
        else
        {
            pourDetector.isPouring = false;
        }
    }

    private float CalculateSpillAngle()
    {
        return Vector3.Angle(transform.up, Vector3.up);
    }
}
