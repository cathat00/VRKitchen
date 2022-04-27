using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  PourDetector is a class used in other scripts (SEE Faucet, Spillable) to generate a stream of liquid (SEE LiquidStream when certain conditions have been met. 
 *  When the pour conditions are no longer met, PourDetector ends the stream of liquid. 
 */

public class PourDetector : MonoBehaviour
{
    public Transform origin; // Origin point for the liquid stream

    [HideInInspector] public bool isPouring; // "Is the PourDetector pouring?" 
    private bool pourCheck; // "Should the PourDetector be pouring?" 
    
    [SerializeField] private GameObject streamPrefab; // Type of liquid to be poured
    private Stream currentStream; // Current stream of liquid being poured
    
    [SerializeField] private float unitsPerSecond = .138f; // Unit output per second. (Liters for liquids, grams for spices)

    private void Awake()
    {
        pourCheck = isPouring;
    }

    // Update is called once per frame
    void Update()
    {
        if (pourCheck != isPouring) // Is the detector pouring? Should it be? 
        {
            if (isPouring)
            {
                StartPour(); // Begin pouring
            }
            else
            {
                EndPour(); // Stop pouring
            }
            pourCheck = isPouring;
        }
    }

    private void StartPour()
    {
        currentStream = CreateStream(); // Create a new stream, store it in current stream
        currentStream.Begin();
    }

    private void EndPour()
    {
        currentStream.End(); // End the current stream
        currentStream = null; // There is no current stream
    }

    private Stream CreateStream() // Create a new liquid stream at the origin point
    {
        GameObject streamObj = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        Stream stream = streamObj.GetComponent<Stream>();

        stream.unitsPerSecond = unitsPerSecond; // Set the unit output of the created stream

        return stream;
    }
}
