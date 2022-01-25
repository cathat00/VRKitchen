using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PourDetector))]
public class Faucet : MonoBehaviour
{
    [SerializeField] GameObject nozzle;
    [SerializeField] float nozzleThreshold = 0.0f;

    PourDetector pourDetect;
    
    // Start is called before the first frame update
    void Start()
    {
        pourDetect = GetComponent<PourDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nozzle.transform.localEulerAngles.x > nozzleThreshold)
        {
            pourDetect.isPouring = true;
        }
        else
        {
            pourDetect.isPouring = false;
        }
    }
}
