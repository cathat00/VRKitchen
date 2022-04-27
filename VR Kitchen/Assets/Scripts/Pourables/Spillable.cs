using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spillable objects create liquid streams when they reach a certain rotation threshold. Stream instantiation is handled by an attached PourDetector (SEE PourDetector class)
 */

public class Spillable : MonoBehaviour
{
    [SerializeField] private float spillThreshold = 45f; // Rotational threshold at which the spillable object will instantiate a liquid stream
    private PourDetector pourDetector; 

    private void Start()
    {
        pourDetector = GetComponent<PourDetector>();
    }

    void Update()
    {
        if (CalculateSpillAngle() > spillThreshold)
        {
            pourDetector.isPouring = true; // Instantiate a stream through pour detector
        }
        else
        {
            pourDetector.isPouring = false; // End pour through pour detector
        }
    }

    private float CalculateSpillAngle()
    {
        return Vector3.Angle(transform.up, Vector3.up); // Angle between worldspace UP vector and object's local UP vector
    }
}
