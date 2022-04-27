using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A faucet creates a stream of liquid when its attached nozzle is rotated beyond a certain threshold. Stream instantiation is handled by the attached PourDetector (SEE PourDetector class)
 */

[RequireComponent(typeof(PourDetector))]
public class Faucet : MonoBehaviour
{
    [SerializeField] GameObject nozzle; // Attached nozzle
    [SerializeField] float nozzleThreshold = 0.0f; // Rotation threshold beyond which faucet turns on

    PourDetector pourDetect;
    
    // Start is called before the first frame update
    void Start()
    {
        pourDetect = GetComponent<PourDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nozzle.transform.localEulerAngles.x > nozzleThreshold) // Check if nozzle rotated beyond threshold
        {
            pourDetect.isPouring = true; // Faucet turns on via PourDetector
        }
        else
        {
            pourDetect.isPouring = false; // Faucet turns off
        }
    }
}
