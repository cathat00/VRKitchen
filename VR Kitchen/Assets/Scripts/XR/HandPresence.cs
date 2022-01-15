using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private InputDeviceCharacteristics deviceCharacteristics;
    [SerializeField] private GameObject modelPrefab;

    public List<GameObject> controllerPrefabs;
    private InputDevice targetDevice;

    private GameObject spawnedHandModel;
    private Animator handAnimator;

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, inputDevices);

        if (inputDevices.Count == 0)
        {
            return;
        }

        targetDevice = inputDevices[0];
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else if (!spawnedHandModel)
        {
            Debug.Log(targetDevice.name + targetDevice.characteristics);
            spawnedHandModel = Instantiate(modelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }

        else // If we have found a target device AND spawned a hand model
        {
            UpdateHandAnimation();
        }
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
