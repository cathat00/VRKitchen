using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    public Hand hand;

    [SerializeField] InputActionReference triggerAction;
    [SerializeField] InputActionReference gripAction;
    
    private XRDirectInteractor interactor;
    bool isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrabbing = (interactor.selectTarget != null);

        if (!isGrabbing)
        {
            hand.SetGrip(gripAction.action.ReadValue<float>());
            hand.SetTrigger(triggerAction.action.ReadValue<float>());
        }
        else
        {
            hand.grabbing();
        }
    }
}
