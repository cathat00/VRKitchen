using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/*
 * Hand Presence locates the VR controllers in the scene, checks their attributes, displays, and animates them accordingly. 
 */

public class HandPresence : MonoBehaviour
{ 
    [SerializeField] private InputDeviceCharacteristics deviceCharacteristics; // Characteristics of the hand controllers
    [SerializeField] private GameObject modelPrefab; // Model to display for player's hands

    public List<GameObject> controllerPrefabs; 
    private InputDevice targetDevice;

    private GameObject spawnedHandModel; // Hand model used
    private Animator handAnimator;

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize() // Try to find hands in the scene and initialize them
    {
        var inputDevices = new List<InputDevice>(); // List of input devices found
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, inputDevices); // Search for a device with given characteristics

        if (inputDevices.Count == 0)
        {
            return;
        }

        targetDevice = inputDevices[0]; // Select the first device that meets the characteristic requirements
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else if (!spawnedHandModel)
        {
            Debug.Log(targetDevice.name + targetDevice.characteristics); // Print found device to console
            spawnedHandModel = Instantiate(modelPrefab, transform); // Create the hand model
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }

        else // If we have found a target device AND spawned a hand model
        {
            UpdateHandAnimation();
        }
    }

    void UpdateHandAnimation() // Generate blended animation for hands
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue); // Blend pinch animation by trigger value
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue); // Blend grip animation by grip value
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
